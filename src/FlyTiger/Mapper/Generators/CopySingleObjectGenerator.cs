using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using static FlyTiger.Mapper.Generators.Utils;

namespace FlyTiger.Mapper.Generators
{
    internal class CopySingleObjectGenerator : ConvertObjectGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var toType = mappingInfo.TargetType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;

            if (!mappingInfo.MapUpdate) return;
            if (toType.IsValueType)
            {
                this.ReportTargetIsValueTypeCanNotCopy(context);
                return;
            }

            codeBuilder.AppendCodeLines(
                $"private static void {mappingInfo.ConvertToMethodName}(this {fromTypeDisplay} source, {toTypeDisplay} target, Action<object> onRemoveItem = null, Action<object> onAddItem = null)");
            codeBuilder.BeginSegment();
            var queue = new CopyToQueue();
            var copyMethod = queue.AddMethod(CopyToMethodType.CopySingle, context);
            codeBuilder.AppendCodeLines($"{copyMethod.InlineMethodName}(source, target);");
            while (queue.HasItem)
            {
                var method = queue.Dequeue();
                switch (method.CopyMethodType)
                {
                    case CopyToMethodType.CopySingle:
                        AddCopyObjectMethodInternal(queue, method);
                        break;
                    case CopyToMethodType.CopyCollection:
                        AddCopyCollectionMethodInternal(queue, method);
                        break;
                    case CopyToMethodType.CopyDictionary:
                        AddCopyDictionaryMethodInternal(queue, method);
                        break;
                    case CopyToMethodType.ConvertSingle:
                        AddConvertObjectMethodInternal(queue, method);
                        break;
                    default:
                        break;
                }
            }
            codeBuilder.EndSegment();
        }

        void AddCopyDictionaryMethodInternal(CopyToQueue queue, CopyToMethodInfo method)
        {
            var context = method.Context;
            var codeBuilder = context.CodeBuilder;
            var methodName = method.InlineMethodName;
            codeBuilder.AppendCodeLines($"void {methodName}({method.Context.MappingInfo.SourceTypeFullDisplay} source, {method.Context.MappingInfo.TargetTypeFullDisplay} target)");
            codeBuilder.BeginSegment();
            var (_, sValue) = GetKeyValueType(method.SourceType as INamedTypeSymbol);
            var (tKey, tValue) = GetKeyValueType(method.TargetType as INamedTypeSymbol);

            if (CanCopyingSubObjectProperty(sValue, tValue, method.Context))
            {
                var newContext = method.Context.Fork(sValue, tValue);
                //加入队列
                var copyMethod = queue.AddMethod(CopyToMethodType.CopySingle, newContext);
                var convertMethod = queue.AddMethod(CopyToMethodType.ConvertSingle, newContext);
                codeBuilder.AppendCodeLines($@"var sourceKeys = source.Keys.ToHashSet();
var targetKeys = target.Keys.ToHashSet();
// modify item
sourceKeys.Where(p => targetKeys.Contains(p)).ForEach(key =>
{{
    if (source[key] == null || target[key] == null) 
    {{
        target[key] = {convertMethod.InlineMethodName}(source[key]);    
    }}
    else
    {{
        {copyMethod.InlineMethodName}(source[key], target[key]);
    }}
}});
// remove item                                            
targetKeys.Except(sourceKeys.Select(p => ({tKey.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})p)).ForEach(p =>
{{
    var item = target[p];
    target.Remove(p);
    onRemoveItem?.Invoke(item);
}});
// add item
sourceKeys.Where(p => !targetKeys.Contains(p)).Select(p => new");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"Key = p,");
                codeBuilder.AppendCodeLines($"Value = {convertMethod.InlineMethodName}(source[p])");
                codeBuilder.EndSegment(@"}).ForEach(p =>
{
    target.Add(p.Key, p.Value);
    onAddItem?.Invoke(p.Value);
});");
            }
            else
            {
                //可以直接赋值,不用调用onAdditem，onRemoveItem
                codeBuilder.AppendCodeLines($@"var sourceKeys = source.Keys.ToHashSet();
var targetKeys = target.Keys.ToHashSet();
// modify item
sourceKeys.Where(p => targetKeys.Contains(p)).ForEach(p =>
{{
    target[p] = source[p];
}});
// remove item                                            
targetKeys.Except(sourceKeys.Select(p => ({tKey.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})p)).ForEach(p =>
{{
    var item = target[p];
    target.Remove(p);
    onRemoveItem?.Invoke(item);
}});
// add item
sourceKeys.Where(p => !targetKeys.Contains(p)).ForEach(p => 
{{
    var item = ({tValue.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})source[p];
    target[p] = item;
    onAddItem?.Invoke(item);
}});");
            }


            codeBuilder.EndSegment();
            (ITypeSymbol, ITypeSymbol) GetKeyValueType(INamedTypeSymbol type)
            {
                return (type.TypeArguments[0], type.TypeArguments[1]);
            }
        }

        void AddCopyCollectionMethodInternal(CopyToQueue queue, CopyToMethodInfo method)
        {
            var context = method.Context;
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = method.SourceType;
            var toType = method.TargetType;
            var fromTypeDisplay = method.Context.MappingInfo.SourceTypeFullDisplay;
            var toTypeDisplay = method.Context.MappingInfo.TargetTypeFullDisplay;
            var methodName = method.InlineMethodName;


            var sourceItemType = GetItemType(fromType);
            var targetItemType = GetItemType(toType);
            var keyMap = EntityKeyFinder.GetEntityKeyMaps(sourceItemType, targetItemType);
            var newContext = method.Context.Fork(sourceItemType, targetItemType);
            //生成对象
            var copyToItemMethod = queue.AddMethod(CopyToMethodType.CopySingle, newContext);
            var targetItemDisplay = targetItemType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            codeBuilder.AppendCodeLines($"void {methodName}({fromTypeDisplay} source, {toTypeDisplay} target)");
            codeBuilder.BeginSegment();
            var sourceItemKeySelector = BuildKeySelectExpression("p", keyMap.SourceKey);
            var targetItemKeySelector = BuildKeySelectExpression("p", keyMap.TargetKey);
            codeBuilder.AppendCodeLines($@"var sourceKeys = source.Select(p => {sourceItemKeySelector}).ToHashSet();
var targetKeys = target.Select(p => {targetItemKeySelector}).ToHashSet();
// modify item
sourceKeys.Intersect(targetKeys).ForEach(key =>
{{
    {copyToItemMethod.InlineMethodName}(
        source.Where(p => key.Equals({sourceItemKeySelector})).First(),
        target.Where(p => key.Equals({targetItemKeySelector})).First());
}});
// remove item
target.Where(p => !sourceKeys.Contains({targetItemKeySelector})).ToList().ForEach(p =>
{{
    target.Remove(p);
    onRemoveItem?.Invoke(p);
}});
// add item
source.Where(p => !targetKeys.Contains({sourceItemKeySelector})).Select(p => new {targetItemDisplay}");
            codeBuilder.BeginSegment();
            AppendPropertyAssign("p", null, ",", newContext);
            codeBuilder.EndSegment(@"}).ForEach(p =>
{
    target.Add(p);
    onAddItem?.Invoke(p);
});");

            codeBuilder.EndSegment();

            string BuildKeySelectExpression(string paramName, IPropertySymbol[] keys)
            {
                if (keys.Length == 1)
                {
                    return $"p.{keys.First().Name}";
                }
                else
                {
                    return $"new {{ {string.Join(", ", keys.Select(k => $"p.{k.Name}"))} }}";
                }
            }
        }
        void AddCopyObjectMethodInternal(CopyToQueue queue, CopyToMethodInfo method)
        {
            var context = method.Context;
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toType = mappingInfo.TargetType;

            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;
            codeBuilder.AppendCodeLines($"void {method.InlineMethodName}({fromTypeDisplay} source, {toTypeDisplay} target)");
            codeBuilder.BeginSegment();
            if (!fromType.IsValueType)
            {
                codeBuilder.AppendCodeLines("if (source == null) return;");
            }
            if (!toType.IsValueType)
            {
                codeBuilder.AppendCodeLines("if (target == null) return;");
            }
            AppendObjectPropertyCopyAssign("source", "target", context, queue);
            codeBuilder.EndSegment();
        }
        void AddConvertObjectMethodInternal(CopyToQueue _, CopyToMethodInfo method)
        {
            var context = method.Context;
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;
            codeBuilder.AppendCodeLines($"{toTypeDisplay} {method.InlineMethodName}({fromTypeDisplay} source)");
            codeBuilder.BeginSegment();
            if (!fromType.IsValueType)
            {
                codeBuilder.AppendCodeLines("if (source == null) return default;");
            }
            codeBuilder.AppendCodeLines($"return new {toTypeDisplay}");
            codeBuilder.BeginSegment();
            AppendPropertyAssign("source", null, ",", context);
            codeBuilder.EndSegment("};");
            codeBuilder.EndSegment();
        }

        private bool CanCopyingSubObjectProperty(ITypeSymbol sourceType, ITypeSymbol targetType, MapperContext convertContext)
        {
            if (convertContext.HasWalked(targetType))
            {
                return false;
            }
            if (targetType.TypeKind != TypeKind.Class || targetType.IsValueType)
            {
                return false;
            }
            return CanMappingSubObjectProperty(sourceType, targetType, convertContext);
        }
        private void AppendObjectPropertyCopyAssign(string sourceRefrenceName, string targetRefrenceName, MapperContext convertContext, CopyToQueue queue)
        {
            var lineSplitChar = ";";
            var mappingInfo = convertContext.MappingInfo;
            var codeBuilder = convertContext.CodeBuilder;
            var targetProps = GetTargetPropertyDictionary(mappingInfo.TargetType);
            var sourceProps = GetSourcePropertyDictionary(mappingInfo.SourceType);

            // custom
            foreach (var cm in mappingInfo.CustomerMappings)
            {
                var actualSourceExpression = cm.Value.Replace("$", sourceRefrenceName);
                codeBuilder.AppendCodeLines(
                    $"{FormatRefrence(targetRefrenceName, cm.Key)} = {actualSourceExpression}{lineSplitChar}");
            }

            var mappingPropKeys = targetProps.Keys.Intersect(sourceProps.Keys)
                .Except(mappingInfo.IgnoreProperties).Except(mappingInfo.CustomerMappings.Keys).ToList();

            var targetNotFilledKeys = targetProps.Keys.Except(mappingInfo.IgnoreProperties)
                .Except(mappingInfo.CustomerMappings.Keys).Except(mappingPropKeys).ToList();

            var sourceNotUsedKeys = sourceProps.Keys.Except(mappingInfo.IgnoreProperties)
                .Except(mappingInfo.CustomerMappings.Keys).Except(mappingPropKeys).ToList();

            // same name
            foreach (var propName in mappingPropKeys)
            {
                var targetProp = targetProps[propName];
                var sourceProp = sourceProps[propName];
                var targetPropType = targetProp.Type;
                var sourcePropType = sourceProp.Type;
                if (targetProp.IsReadOnly)
                {
                    if (!sourceProp.IsReadOnly)
                    {
                        this.ReportReadOnlyPropertyCanNotFilled(convertContext, targetProp, sourceProp);
                    }
                    continue;
                }
                if (targetProp.SetMethod != null && targetProp.SetMethod.IsInitOnly)
                {
                    //TOTO 
                    this.ReportInitOnlyPropertyCanNotCopyValue(convertContext, targetProp, sourceProp);
                    continue;
                }
                if (CanCopyingDictionaryProperty(sourcePropType, targetPropType, convertContext))
                {
                    var copyDicMethod = queue.AddMethod(CopyToMethodType.CopyDictionary, convertContext.Fork(sourcePropType, targetPropType));
                    AppendDictionaryCopyAssign(FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), copyDicMethod);
                }
                else if (CanCopyingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection copy
                    var copyCollectionMethod = queue.AddMethod(CopyToMethodType.CopyCollection, convertContext.Fork(sourcePropType, targetPropType));
                    AppendCollectionCopyAssign(FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), copyCollectionMethod);

                }
                else if (CanMappingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection new object
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingNewCollection(newConvertContext, FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), lineSplitChar);
                }
                else if (CanCopyingSubObjectProperty(sourcePropType, targetPropType, convertContext))
                {
                    // sub object copy
                    var copyObjectMethod = queue.AddMethod(CopyToMethodType.CopySingle, convertContext.Fork(sourcePropType, targetPropType));
                    AppendSubObjectCopyAssign(FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), copyObjectMethod);
                }
                else if (CanAssign(sourcePropType, targetPropType, convertContext))
                {
                    // default 
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, propName)} = {FormatRefrence(sourceRefrenceName, propName)}{lineSplitChar}");
                }
                else if (CanMappingSubObjectProperty(sourcePropType, targetPropType, convertContext))
                {
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingNewSubObject(newConvertContext, FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), "=", lineSplitChar);

                }
                else
                {
                    this.ReportPropertyCanNotBeMapped(convertContext, targetProp, sourceProp);
                }

            }
            // UserName = User.Name
            foreach (var prop in targetProps.Where(p => targetNotFilledKeys.Contains(p.Key)))
            {
                if (HasSuggestionPath(prop, sourceProps, out var paths, convertContext))
                {
                    sourceNotUsedKeys.Remove(paths.First().Name);

                    var actualSourceExpression = $"{sourceRefrenceName}.{string.Join(".", paths.Select(p => p.Name))}";
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, prop.Key)} = {actualSourceExpression}{lineSplitChar}");
                }
                else
                {
                    this.ReportTargetPrpertyNotFilled(convertContext, prop.Value);
                }
            }
            foreach (var prop in sourceProps.Where(p => sourceNotUsedKeys.Contains(p.Key)))
            {
                this.ReportSourcePropertyNotMapped(convertContext, prop.Value);
            }
        }
        private bool CanCopyingDictionaryProperty(ITypeSymbol sourcePropType, ITypeSymbol targetPropertyType, MapperContext context)
        {
            if (IsDictionary(sourcePropType) && IsDictionary(targetPropertyType))
            {
                var (sKey, sValue) = GetKeyValueType(sourcePropType);
                var (tKey, tValue) = GetKeyValueType(targetPropertyType);
                if (CanAssign(sKey, tKey, context))
                {
                    return CanAssign(sValue, tValue, context) || CanCopyingSubObjectProperty(sValue, tValue, context);
                }
            }
            return false;
        }
        private bool CanCopyingCollectionProperty(ITypeSymbol sourcePropType, ITypeSymbol targetPropType, MapperContext convertContext)
        {
            if (SourceTypeIsEnumerable() && TargetTypeIsGenericCollection())
            {
                var sourceItemType = GetItemType(sourcePropType);
                var targetItemType = GetItemType(targetPropType);
                var keyMap = EntityKeyFinder.GetEntityKeyMaps(sourceItemType, targetItemType);
                return keyMap != null && CanCopyingSubObjectProperty(sourceItemType, targetItemType, convertContext);
            }

            bool SourceTypeIsEnumerable()
            {
                if (sourcePropType is IArrayTypeSymbol)
                {
                    return true;
                }

                if (sourcePropType is INamedTypeSymbol namedSourcePropType)
                {
                    if (namedSourcePropType.IsGenericType)
                    {
                        if (namedSourcePropType.ConstructUnboundGenericType().SafeEquals(typeof(IEnumerable<>)))
                        {
                            return true;
                        }

                        if (sourcePropType.AllInterfaces.Any(p =>
                                p.IsGenericType && p.ConstructUnboundGenericType().SafeEquals(typeof(IEnumerable<>))))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            bool TargetTypeIsGenericCollection()
            {

                if (targetPropType is INamedTypeSymbol namedTargetPropType)
                {
                    if (namedTargetPropType.IsGenericType)
                    {
                        var targetUnboundGenericType = namedTargetPropType.ConstructUnboundGenericType();
                        return
                            targetUnboundGenericType.SafeEquals(typeof(IList<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(List<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(ICollection<>));
                    }
                }

                return false;
            }

            return false;
        }
        private void AppendDictionaryCopyAssign(string sourceRefrenceName, string targetRefrenceName, CopyToMethodInfo copyToMethod)
        {
            var context = copyToMethod.Context;
            var codeBuilder = context.CodeBuilder;
            codeBuilder.AppendCodeLines($"if ({sourceRefrenceName} == null || {targetRefrenceName} == null)");
            codeBuilder.BeginSegment();
            //直接给集合赋值
            MappingNewDictionary(context, sourceRefrenceName, targetRefrenceName, ";");
            codeBuilder.EndSegment();
            codeBuilder.AppendCodeLines("else");
            codeBuilder.BeginSegment();
            codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, {targetRefrenceName});");
            codeBuilder.EndSegment();
        }
        private void AppendCollectionCopyAssign(string sourceRefrenceName, string targetRefrenceName, CopyToMethodInfo copyToMethod)
        {
            var context = copyToMethod.Context;
            var codeBuilder = context.CodeBuilder;
            codeBuilder.AppendCodeLines($"if ({sourceRefrenceName} == null || {targetRefrenceName} == null)");
            codeBuilder.BeginSegment();
            //直接给集合赋值
            MappingNewCollection(context, sourceRefrenceName, targetRefrenceName, ";");
            codeBuilder.EndSegment();
            codeBuilder.AppendCodeLines("else");
            codeBuilder.BeginSegment();
            codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, {targetRefrenceName});");
            codeBuilder.EndSegment();

        }
        private void AppendSubObjectCopyAssign(string sourceRefrenceName, string targetRefrenceName, CopyToMethodInfo copyToMethod)
        {
            var context = copyToMethod.Context;
            var codeBuilder = context.CodeBuilder;
            var mappingInfo = context.MappingInfo;
            if (!mappingInfo.SourceType.IsValueType)
            {
                codeBuilder.AppendCodeLines($"if ({sourceRefrenceName} == null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"{targetRefrenceName} = default;");
                codeBuilder.EndSegment();
                codeBuilder.AppendCodeLines("else");
                codeBuilder.BeginSegment();
                if (mappingInfo.TargetType.IsValueType)
                {
                    codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, ref {targetRefrenceName})");
                }
                else
                {
                    codeBuilder.AppendCodeLines($"if ({targetRefrenceName} == null)");
                    codeBuilder.BeginSegment();
                    //创建新对象
                    MappingNewSubObject(context, sourceRefrenceName, targetRefrenceName, "=", ";");
                    codeBuilder.EndSegment();
                    codeBuilder.AppendCodeLines("else");
                    codeBuilder.BeginSegment();
                    //复制
                    codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, {targetRefrenceName});");
                    codeBuilder.EndSegment();
                }
                codeBuilder.EndSegment();
            }
            else
            {
                if (mappingInfo.TargetType.IsValueType)
                {
                    codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, ref {targetRefrenceName});");
                }
                else
                {
                    codeBuilder.AppendCodeLines($"if ({targetRefrenceName} == null)");
                    codeBuilder.BeginSegment();
                    //创建新的对象
                    MappingNewSubObject(context, sourceRefrenceName, targetRefrenceName, "=", ";");

                    codeBuilder.EndSegment();
                    codeBuilder.AppendCodeLines("else");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{copyToMethod.InlineMethodName}({sourceRefrenceName}, {targetRefrenceName});");
                    codeBuilder.EndSegment();
                }
            }
        }
    }
}
