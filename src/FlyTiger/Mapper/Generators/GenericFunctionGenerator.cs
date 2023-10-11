using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper.Generators
{
    internal class GenericFunctionGenerator
    {
        public void AppendFunctions(CodeWriter codeWriter,
           CsharpCodeBuilder codeBuilder, IList<AttributeData> attributeDatas)
        {
            var allSources = attributeDatas.Where(p => p.AttributeClass.Is(MapperGenerator.AttributeFullName))
                .Select(p => ConvertMappingInfo.FromAttributeData(p))
                .ToLookup(p => (p.SourceType, p.SourceTypeFullDisplay));
            if (allSources.Any())
            {
                AppendCacheFunctions(codeBuilder);
            }
            foreach (var item in allSources)
            {
                AppendGenericFunctions(item.Key, item.ToList(), codeBuilder);
            }
            if (allSources.Any())
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
            //convert
            AddToMethodForSingle();
            AddToMethodForSingleWithPostAction();
            AddToMethodForEnumable();
            AddToMethodForEnumableWithPostAction();
            //update
            AddCopyToMethodForSingle();
            AddCopyToMethodForCollection();
            AddCopyToMethodForCollection2();
            AddCopyToMethodForCollection3();
            //query
            AddToMethodForQueryable();

            void AddToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static T {methodName}<T>(this {fromType.Display} source) where T:new()");
                codeBuilder.BeginSegment();
                if (!fromType.Type.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return default;");
                }

                foreach (var mapping in mappingInfos)
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
                   $"public static T {methodName}<T>(this {fromType.Display} source, Action<T> postHandler) where T: class, new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"var result = source.{methodName}<T>();");
                codeBuilder.AppendCodeLines("postHandler?.Invoke(result);");
                codeBuilder.AppendCodeLines("return result;");
                codeBuilder.EndSegment();
            }

            void AddCopyToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static void {methodName}<T>(this {fromType.Display} source, T target, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T:class");
                codeBuilder.BeginSegment();
                if (!fromType.Type.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return;");
                }
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, ({mapping.TargetTypeFullDisplay})(object)target, onRemoveItem, onAddItem);");
                    codeBuilder.AppendCodeLines($"return;");
                    codeBuilder.EndSegment();
                }

                AppendNotSupportedExceptionAndEndSegment();
            }
            void AddCopyToMethodForCollection()
            {
                codeBuilder.AppendCodeLines(
               $"public static void {methodName}<T, K>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Func<{fromType.Display}, K> sourceItemKeySelector, Func<T, K> targetItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T: class, new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines(@"_ = sourceItemKeySelector ?? throw new ArgumentNullException(nameof(sourceItemKeySelector));
_ = targetItemKeySelector ?? throw new ArgumentNullException(nameof(targetItemKeySelector));");
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{mapping.ConvertToMethodName}(source, (ICollection<{mapping.TargetTypeFullDisplay}>)target, updateMode, sourceItemKeySelector, (Func<{mapping.TargetTypeFullDisplay}, K>)targetItemKeySelector, onRemoveItem, onAddItem);");
                    codeBuilder.AppendCodeLines("return;");
                    codeBuilder.EndSegment();
                }
                AppendNotSupportedExceptionAndEndSegment();
            }
            void AddCopyToMethodForCollection2()
            {
                codeBuilder.AppendCodeLines(
               $"public static void {methodName}<T, K>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Expression<Func<{fromType.Display}, K>> sourceItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T: class, new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"_ = sourceItemKeySelector ?? throw new ArgumentNullException(nameof(sourceItemKeySelector));
var sourceFunc = GetSourceKeySelectorFunc(sourceItemKeySelector);
var targetFunc = GetTargetKeySelectorFunc<{fromType.Display}, T, K>(sourceItemKeySelector);
source.To(target, updateMode, sourceFunc, targetFunc, onRemoveItem, onAddItem);");
                codeBuilder.EndSegment();
            }
            void AddCopyToMethodForCollection3()
            {
                var keyProperty = EntityKeyFinder.GetEntityKey(fromType.Type);
                if (keyProperty != null)
                {
                    codeBuilder.AppendCodeLines(
             $"public static void {methodName}<T>(this IEnumerable<{fromType.Display}> source, ICollection<T> target, CollectionUpdateMode updateMode, Action<object> onRemoveItem = null, Action<object> onAddItem = null) where T: class, new()");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($@"source.To(target, updateMode, p => p.{keyProperty.Name}, onRemoveItem, onAddItem);");
                    codeBuilder.EndSegment();
                }


            }

            void AddToMethodForEnumable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromType.Display}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
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
                codeBuilder.AppendCodeLines("return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);");
                codeBuilder.EndSegment();
            }

            void AddToMethodForQueryable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IQueryable<T> {methodName}<T>(this IQueryable<{fromType.Display}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({mapping.TargetTypeFullDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (IQueryable<T>){mapping.ConvertToMethodName}(source);");
                    codeBuilder.EndSegment();
                }
                AppendNotSupportedExceptionAndEndSegment();
            }
            void AppendNotSupportedExceptionAndEndSegment()
            {
                codeBuilder.AppendCodeLines($"throw new NotSupportedException($\"Can not convert '{{typeof({fromType.Display})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
        }
    }
}
