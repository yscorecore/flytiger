using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper.Generators
{

    internal class GenericFunctionGenerator
    {

        public void AppendFunctions(CodeWriter _,
          CsharpCodeBuilder codeBuilder, List<ConvertMappingInfo> rootConvertMappingInfos)
        {
            if (rootConvertMappingInfos.Any())
            {
                AppendCacheFunctions(codeBuilder);
            }
            var sources = rootConvertMappingInfos.ToLookup(p => (p.SourceType, p.SourceTypeFullDisplay));
            foreach (var item in sources)
            {
                AppendGenericFunctions(item.Key, item.ToList(), codeBuilder);
            }
            if (rootConvertMappingInfos.Any())
            {
                AppendInternalClass(codeBuilder);
            }
        }

        private void AppendCacheFunctions(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines(@"private static readonly System.Collections.Concurrent.ConcurrentDictionary<int, Delegate> sourceKeySelectorCache = new System.Collections.Concurrent.ConcurrentDictionary<int, Delegate>();
private static Func<Source, Key> GetSourceKeySelectorFunc<Source, Key>(Expression<Func<Source, Key>> sourceKeyLambda)
{
    int cacheKey = typeof(Source).GetHashCode() ^ sourceKeyLambda.ToString().GetHashCode();
    return (Func<Source, Key>)sourceKeySelectorCache.GetOrAdd(cacheKey, (_) =>
    {
        return sourceKeyLambda.Compile();
    });
}

private static readonly System.Collections.Concurrent.ConcurrentDictionary<int, Delegate> targetKeySelectorCache = new System.Collections.Concurrent.ConcurrentDictionary<int, Delegate>();
private static Func<Target, Key> GetTargetKeySelectorFunc<Source, Target, Key>(Expression<Func<Source, Key>> sourceKeyLambda)
{
    int cacheKey = typeof(Source).GetHashCode() ^ typeof(Target).GetHashCode() ^ sourceKeyLambda.ToString().GetHashCode();
    return (Func<Target, Key>)targetKeySelectorCache.GetOrAdd(cacheKey, (_) =>
    {
        var newParameter = Expression.Parameter(typeof(Target), ""p"");
        var replacer = new ParameterExpressionReplacer(newParameter);
        var newLambdaBody = replacer.Visit(sourceKeyLambda.Body);
        var newLambda = Expression.Lambda<Func<Target, Key>>(newLambdaBody, newParameter);
        return newLambda.Compile();
    });
}");
        }
        private void AppendInternalClass(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines(@"private class ParameterExpressionReplacer : ExpressionVisitor
{
    public ParameterExpressionReplacer(ParameterExpression newParameter)
    {
        this.newParameter = newParameter;
    }
    private readonly ParameterExpression newParameter;

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression.NodeType == ExpressionType.Parameter)
        {
            return Expression.PropertyOrField(newParameter, node.Member.Name);
        }
        else
        {
            return base.VisitMember(node);
        }
    }
}");
        }
        private void AppendGenericFunctions((ITypeSymbol Type, string Display) fromType, List<ConvertMappingInfo> mappingInfos,
            CsharpCodeBuilder codeBuilder)
        {
            var methodName = "To";
            var toConvertMappings = mappingInfos.Where(p => p.MapConvert).ToList();
            //TODO temp ignore value type
            var toUpdateMappings = mappingInfos.Where(p => p.MapUpdate).Where(p => !p.TargetType.IsValueType).ToList();
            var toBatchUpdateMappings = mappingInfos.Where(p => p.MapUpdate && p.MapConvert).Where(p => !p.TargetType.IsValueType).ToList();
            var toQueryMappings = mappingInfos.Where(p => p.MapQuery).ToList();

            //convert
            if (toConvertMappings.Any())
            {
                AppendConvertFunctions(toConvertMappings);
            }

            //update
            if (toUpdateMappings.Any())
            {
                AppendUpdateFunctions(toUpdateMappings);
            }
            //batch update
            if (toBatchUpdateMappings.Any())
            {
                AppendBatchUpdateFunctions(toBatchUpdateMappings);
            }

            //query
            if (toQueryMappings.Any())
            {
                AppendQueryFunctions(toQueryMappings);
            }
            void AppendConvertFunctions(List<ConvertMappingInfo> mappings)
            {
                AddToMethodForSingle();
                AddToMethodForSingleWithPostAction();
                AddToMethodForEnumable();
                AddToMethodForEnumableWithPostAction();
                AddToMethodForDictionary();
                AddToMethodForDictonaryWithPostAction();

                void AddToMethodForSingle()
                {
                    codeBuilder.AppendCodeLines(
                        $"public static T {methodName}<T>(this {fromType.Display} source) where T : new()");
                    codeBuilder.BeginSegment();
                    if (!fromType.Type.IsValueType)
                    {
                        codeBuilder.AppendCodeLines("if (source == null) return default;");
                    }

                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"return (T)(object){mapping.ConvertToMethodName}(source);");
                        codeBuilder.EndSegment();
                    }

                    AppendNotSupportedExceptionAndEndSegment();
                }
                void AddToMethodForSingleWithPostAction()
                {
                    codeBuilder.AppendCodeLines(
                       $"public static T {methodName}<T>(this {fromType.Display} source, Action<T> postHandler) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"var result = source.{methodName}<T>();");
                    codeBuilder.AppendCodeLines("postHandler?.Invoke(result);");
                    codeBuilder.AppendCodeLines("return result;");
                    codeBuilder.EndSegment();
                }
                void AddToMethodForEnumable()
                {
                    codeBuilder.AppendCodeLines(
                        $"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromType.Display}> source) where T : new()");
                    codeBuilder.BeginSegment();
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"return (IEnumerable<T>)source?.Select(p => p.{mapping.ConvertToMethodName}());");
                        codeBuilder.EndSegment();
                    }

                    AppendNotSupportedExceptionAndEndSegment();
                }
                void AddToMethodForEnumableWithPostAction()
                {

                    codeBuilder.AppendCodeLines($"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromType.Display}> source, Action<T> postHandler) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return source == null || postHandler == null ? source.{methodName}<T>() : source.To<T>().EachItem(postHandler);");
                    codeBuilder.EndSegment();
                }

                void AddToMethodForDictionary()
                {
                    codeBuilder.AppendCodeLines(
                        $"public static IDictionary<Key, T> {methodName}<Key, T>(this IDictionary<Key, {fromType.Display}> source) where T : new()");
                    codeBuilder.BeginSegment();
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"return (IDictionary<Key, T>)(source?.ToDictionary(p => p.Key, p => p.Value.{mapping.ConvertToMethodName}()));");
                        codeBuilder.EndSegment();
                    }
                    AppendNotSupportedExceptionAndEndSegment();
                }
                void AddToMethodForDictonaryWithPostAction()
                {

                    codeBuilder.AppendCodeLines($"public static IDictionary<Key, T> {methodName}<Key, T>(this IDictionary<Key, {fromType.Display}> source, Action<T> postHandler) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"var res = source?.{methodName}<Key, T>();");
                    codeBuilder.AppendCodeLines($"res?.ForEach(p => postHandler?.Invoke(p.Value));");
                    codeBuilder.AppendCodeLines($"return res;");
                    codeBuilder.EndSegment();
                }

            }

            void AppendUpdateFunctions(List<ConvertMappingInfo> mappings)
            {
                AddCopyToMethodForSingle();

                void AddCopyToMethodForSingle()
                {
                    codeBuilder.AppendCodeLines(
                        $"public static void {methodName}<T>(this {fromType.Display} source, T target, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T : class");
                    codeBuilder.BeginSegment();
                    if (!fromType.Type.IsValueType)
                    {
                        codeBuilder.AppendCodeLines("_ = source ?? throw new ArgumentNullException(nameof(source));");
                    }
                    codeBuilder.AppendCodeLines("_ = target ?? throw new ArgumentNullException(nameof(target));");
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, ({mapping.TargetTypeFullDisplay})(object)target, onRemoveItem, onAddItem);");
                        codeBuilder.AppendCodeLines($"return;");
                        codeBuilder.EndSegment();
                    }

                    AppendNotSupportedExceptionAndEndSegment();
                }
            }

            void AppendBatchUpdateFunctions(List<ConvertMappingInfo> mappings)
            {
                AddCopyToMethodForDictionary();
                AddCopyToMethodForCollection();
                AddCopyToMethodForCollection2();
                AddCopyToMethodForCollection3();

                void AddCopyToMethodForCollection()
                {
                    codeBuilder.AppendCodeLines(
                   $"public static void {methodName}<T, K>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Func<{fromType.Display}, K> sourceItemKeySelector, Func<T, K> targetItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines(@"_ = source ?? throw new ArgumentNullException(nameof(source));
_ = target ?? throw new ArgumentNullException(nameof(target));
_ = sourceItemKeySelector ?? throw new ArgumentNullException(nameof(sourceItemKeySelector));
_ = targetItemKeySelector ?? throw new ArgumentNullException(nameof(targetItemKeySelector));");
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, (ICollection<{mapping.TargetTypeFullDisplay}>)target, updateMode, sourceItemKeySelector, (Func<{mapping.TargetTypeFullDisplay}, K>)targetItemKeySelector, onRemoveItem, onAddItem);");
                        codeBuilder.AppendCodeLines("return;");
                        codeBuilder.EndSegment();
                    }
                    AppendNotSupportedExceptionAndEndSegment();
                }
                void AddCopyToMethodForDictionary()
                {
                    codeBuilder.AppendCodeLines(
                   $"public static void {methodName}<Key, T>(this IDictionary<Key, {fromType.Display}> source, IDictionary<Key, T> target, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines(@"_ = source ?? throw new ArgumentNullException(nameof(source));
_ = target ?? throw new ArgumentNullException(nameof(target));");
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, (IDictionary<Key, {mapping.TargetTypeFullDisplay}>)target, onRemoveItem, onAddItem);");
                        codeBuilder.AppendCodeLines("return;");
                        codeBuilder.EndSegment();
                    }
                    AppendNotSupportedExceptionAndEndSegment();
                }
                void AddCopyToMethodForCollection2()
                {
                    codeBuilder.AppendCodeLines(
                   $"public static void {methodName}<T, K>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Expression<Func<{fromType.Display}, K>> sourceItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T : class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($@"_ = sourceItemKeySelector ?? throw new ArgumentNullException(nameof(sourceItemKeySelector));
var sourceFunc = GetSourceKeySelectorFunc(sourceItemKeySelector);
var targetFunc = GetTargetKeySelectorFunc<{fromType.Display}, T, K>(sourceItemKeySelector);
source.To(target, updateMode, sourceFunc, targetFunc, onRemoveItem, onAddItem);");
                    codeBuilder.EndSegment();
                }
                void AddCopyToMethodForCollection3()
                {
                    var keyPropertys = EntityKeyFinder.GetEntityKey(fromType.Type);
                    if (keyPropertys != null)
                    {
                        codeBuilder.AppendCodeLines(
                 $"public static void {methodName}<T>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T : class, new()");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($@"source.To(target, updateMode, {BuildKeySelectExpression(keyPropertys)}, onRemoveItem, onAddItem);");
                        codeBuilder.EndSegment();
                    }
                    string BuildKeySelectExpression(IPropertySymbol[] keys)
                    {
                        if (keys.Length == 1)
                        {
                            return $"p => p.{keys.First().Name}";
                        }
                        else
                        {
                            return $"p => new {{ {string.Join(", ", keys.Select(k => $"p.{k.Name}"))} }}";
                        }
                    }

                }
            }

            void AppendQueryFunctions(List<ConvertMappingInfo> mappings)
            {
                AddToMethodForQueryable();
                void AddToMethodForQueryable()
                {
                    codeBuilder.AppendCodeLines(
                        $"public static IQueryable<T> {methodName}<T>(this IQueryable<{fromType.Display}> source) where T : new()");
                    codeBuilder.BeginSegment();
                    foreach (var mapping in mappings)
                    {
                        codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                        codeBuilder.BeginSegment();
                        codeBuilder.AppendCodeLines($"return (IQueryable<T>){mapping.ConvertToMethodName}(source);");
                        codeBuilder.EndSegment();
                    }
                    AppendNotSupportedExceptionAndEndSegment();
                }
            }

            void AppendNotSupportedExceptionAndEndSegment()
            {
                codeBuilder.AppendCodeLines($"throw new NotSupportedException($\"Can not convert '{{typeof({fromType.Display})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
        }
    }
}
