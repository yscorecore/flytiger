using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FlyTiger.Mapper.Generators
{
    internal class ConvertFunctionGenerator
    {

        public void AppendFunctions(CodeWriter codeWriter,
            CsharpCodeBuilder codeBuilder, IList<AttributeData> attributeDatas)
        {
            //System.Diagnostics.Debugger.Launch();
            foreach (var convertToAttr in attributeDatas)
            {
                var convertMappingInfo = ConvertMappingInfo.FromAttributeData(convertToAttr);
                if (ConvertMappingInfo.CanMappingSubObject(convertMappingInfo.SourceType,
                        convertMappingInfo.TargetType))
                {
                    AppendConvertToFunctions(new MapperContext(codeWriter, codeBuilder, convertMappingInfo, attributeDatas));
                }
                else
                {
                    // TOTO report error
                }
            }
        }


        private void AppendConvertToFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toTypeDisplay = mappingInfo.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var fromTypeDisplay = mappingInfo.SourceType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            //convert
            AddToMethodForSingle(); //dto2entity    ,all dto readable properties should be use
            //update single
            AddCopyToMethodForSingle();
            // update collection
            AddCopyToMethodForCollection(); //dto2entity, all dto readable properties should be use
            //query
            AddToMethodForQueryable(); //entity2dto, all dto writeable properties should be mapped 

            void AddToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"private static {toTypeDisplay} {mappingInfo.ConvertToMethodName}(this {fromTypeDisplay} source)");
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

            void AddCopyToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"private static void {mappingInfo.ConvertToMethodName}(this {fromTypeDisplay} source, {toTypeDisplay} target, Action<object> onRemoveItem = null, Action<object> onAddItem = null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"{BuildCopyObjectMethodName(context.MappingInfo.SourceType, context.MappingInfo.TargetType)}(source, target);");
                CopyToQueue queue = new CopyToQueue(context);
                while (queue.HasItem)
                {
                    var method = queue.Dequeue();
                    if (method.IsCollection)
                    {
                        AddCopyCollectionMethodInternal(queue, method);
                    }
                    else
                    {
                        AddCopyObjectMethodInternal(queue, method);
                    }
                }
                codeBuilder.EndSegment();
            }
            void AddCopyToMethodForCollection()
            {
                codeBuilder.AppendCodeLines(
                    $"private static void {mappingInfo.ConvertToMethodName}<T>(this IEnumerable<{fromTypeDisplay}> source, ICollection<{toTypeDisplay}> target, CollectionUpdateMode updateMode, Func<{fromTypeDisplay}, T> sourceItemKeySelector, Func<{toTypeDisplay}, T> targetItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"if (updateMode == CollectionUpdateMode.Append)
{{
    source.Select(p => p.{mappingInfo.ConvertToMethodName}()).ForEach(p =>
    {{
        target.Add(p);
        onAddItem?.Invoke(p);
    }});
}}
else");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"var sourceKeys = source.Select(sourceItemKeySelector).ToHashSet();
var targetKeys = target.Select(targetItemKeySelector).ToHashSet();
// modify item
sourceKeys.Intersect(targetKeys).ForEach(key =>
{{
    {mappingInfo.ConvertToMethodName}(
        source.Where(p => sourceItemKeySelector(p).Equals(key)).First(),
        target.Where(p => targetItemKeySelector(p).Equals(key)).First(),
        onRemoveItem, onAddItem);
}});
// remove item
if (updateMode == CollectionUpdateMode.Update)
{{
    target.Where(p => !sourceKeys.Contains(targetItemKeySelector(p))).ToList().ForEach(p =>
    {{
        target.Remove(p);
        onRemoveItem?.Invoke(p);
    }});
}}
// add item
source.Where(p => !targetKeys.Contains(sourceItemKeySelector(p))).Select(p => p.{mappingInfo.ConvertToMethodName}()).ForEach(p =>
{{
    target.Add(p);
    onAddItem?.Invoke(p);
}});");

                codeBuilder.EndSegment();
                codeBuilder.EndSegment();
            }


            void AddToMethodForQueryable()
            {
                codeBuilder.AppendCodeLines(
                    $"private static IQueryable<{toTypeDisplay}> {mappingInfo.ConvertToMethodName}(this IQueryable<{fromTypeDisplay}> source)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"return source?.Select(p => new {toTypeDisplay}");
                codeBuilder.BeginSegment();
                AppendPropertyAssign("p", null, ",", context);
                codeBuilder.EndSegment("}).RebuildWithIncludeForEfCore();");
                codeBuilder.EndSegment();
            }
        }

        private static string BuildCopyObjectMethodName(ITypeSymbol source, ITypeSymbol target)
        {
            var sourceName = new string(source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            var targetName = new string(target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return $"Copy{sourceName}To{targetName}";
        }
        private static string BuildCopyCollectionMethodName(ITypeSymbol source, ITypeSymbol target)
        {
            return BuildCopyObjectMethodName(source, target);
        }


        void AddCopyCollectionMethodInternal(CopyToQueue queue, CopyToMethodInfo method)
        {
            var context = method.Context;
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = method.SourceType;
            var toType = method.TargetType;
            var fromTypeDisplay = fromType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var toTypeDisplay = toType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var methodName = BuildCopyCollectionMethodName(mappingInfo.SourceType, mappingInfo.TargetType);


            var sourceItemType = GetItemType(fromType);
            var targetItemType = GetItemType(toType);
            var keyMap = EntityKeyFinder.GetEntityKeyMaps(sourceItemType, targetItemType);
            var newContext = method.Context.Fork(sourceItemType, targetItemType);
            //生成对象
            queue.AddObjectCopyMethod(newContext);

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
    {BuildCopyObjectMethodName(sourceItemType, targetItemType)}(
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
            var toTypeDisplay = mappingInfo.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var fromTypeDisplay = mappingInfo.SourceType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (toType.IsValueType)
            {
                codeBuilder.AppendCodeLines($"void {BuildCopyObjectMethodName(fromType, toType)}({fromTypeDisplay} source, ref {toTypeDisplay} target)");
            }
            else
            {
                codeBuilder.AppendCodeLines($"void {BuildCopyObjectMethodName(fromType, toType)}({fromTypeDisplay} source, {toTypeDisplay} target)");
            }

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

        private bool CanMappingSubObjectProperty(ITypeSymbol sourceType, ITypeSymbol targetType,
            MapperContext convertContext)
        {
            if (convertContext.HasWalked(targetType))
            {
                return false;
            }
            if (sourceType.IsPrimitive() || targetType.IsPrimitive())
            {
                return false;
            }

            if (targetType is INamedTypeSymbol namedTargetType)
            {
                if (sourceType.TypeKind == TypeKind.Class || sourceType.TypeKind == TypeKind.Struct)
                {
                    if (targetType.TypeKind == TypeKind.Struct) return true;
                    return targetType.TypeKind == TypeKind.Class && !targetType.IsAbstract &&
                           namedTargetType.HasEmptyCtor();
                }
            }

            return false;
        }

        private bool CanMappingCollectionProperty(ITypeSymbol sourcePropType, ITypeSymbol targetPropType,
            MapperContext convertContext)
        {
            if (SourceTypeIsEnumerable() && TargetTypeIsSupportedEnumerable())
            {
                var sourceItemType = GetItemType(sourcePropType);
                var targetItemType = GetItemType(targetPropType);
                return CanAssign(sourceItemType, targetItemType, convertContext) ||
                       CanMappingSubObjectProperty(sourceItemType, targetItemType, convertContext);
            }

            return false;

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

            bool TargetTypeIsSupportedEnumerable()
            {
                if (targetPropType is IArrayTypeSymbol)
                {
                    return true;
                }

                if (targetPropType is INamedTypeSymbol namedTargetPropType)
                {
                    if (namedTargetPropType.IsGenericType)
                    {
                        var targetUnboundGenericType = namedTargetPropType.ConstructUnboundGenericType();
                        return
                            targetUnboundGenericType.SafeEquals(typeof(IList<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(List<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(IEnumerable<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(IQueryable<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(ICollection<>));
                    }
                }

                return false;
            }
        }

        private INamedTypeSymbol GetItemType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
            {
                return arrayTypeSymbol.ElementType as INamedTypeSymbol;
            }

            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                return namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol;
            }

            return null;
        }

        private void MappingSubObjectProperty(MapperContext convertContext, string sourceRefrenceName,
            string targetRefrenceName, string propertyName, string lineSplitChar)
        {
            var targetPropertyType = convertContext.MappingInfo.TargetType;
            var sourcePropertyType = convertContext.MappingInfo.SourceType;
            var codeBuilder = convertContext.CodeBuilder;
            var targetPropertyTypeText = targetPropertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var targetPropertyExpression = FormatRefrence(targetRefrenceName, propertyName);
            var sourcePropertyExpression = FormatRefrence(sourceRefrenceName, propertyName);
            if (sourcePropertyType.IsValueType)
            {
                codeBuilder.AppendCodeLines($"{targetPropertyExpression} = new {targetPropertyTypeText}");
            }
            else
            {
                codeBuilder.AppendCodeLines(
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default({targetPropertyTypeText}) : new {targetPropertyTypeText}");
            }

            codeBuilder.BeginSegment();
            AppendPropertyAssign(sourcePropertyExpression, null, ",", convertContext);
            codeBuilder.EndSegment("}" + lineSplitChar);
        }

        private void MappingCollectionProperty(MapperContext convertContext,
            string sourceRefrenceName, string targetRefrenceName, string propertyName, string lineSplitChar)
        {
            var codeBuilder = convertContext.CodeBuilder;
            var targetPropertyType = convertContext.MappingInfo.TargetType;
            var sourcePropertyType = convertContext.MappingInfo.SourceType;
            var targetItemType = GetItemType(targetPropertyType);
            var sourceItemType = GetItemType(sourcePropertyType);
            var targetItemTypeText = targetItemType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            var targetPropertyExpression = FormatRefrence(targetRefrenceName, propertyName);
            var sourcePropertyExpression = FormatRefrence(sourceRefrenceName, propertyName);

            if (sourceItemType.SafeEquals(targetItemType))
            {
                codeBuilder.AppendCodeLines(
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? null : {sourcePropertyExpression}.{ToTargetMethodName()}(){lineSplitChar}");
            }
            else if (CanAssign(sourceItemType, targetItemType, convertContext))
            {
                codeBuilder.AppendCodeLines(
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? null : {sourcePropertyExpression}.Cast<{targetItemTypeText}>().{ToTargetMethodName()}(){lineSplitChar}");
            }
            else
            {
                if (sourceItemType.IsValueType)
                {
                    codeBuilder.AppendCodeLines(
                        $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? null : {sourcePropertyExpression}.Select(p => new {targetItemTypeText}");
                }
                else
                {
                    codeBuilder.AppendCodeLines(
                        $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? null : {sourcePropertyExpression}.Select(p => p == null ? default({targetItemTypeText}) : new {targetItemTypeText}");
                }

                codeBuilder.BeginSegment();
                var newConvertContext = convertContext.Fork(sourceItemType, targetItemType);
                AppendPropertyAssign("p", null, ",", newConvertContext);
                codeBuilder.EndSegment("})." + $"{ToTargetMethodName()}(){lineSplitChar}");
            }

            string ToTargetMethodName()
            {
                if (targetPropertyType is IArrayTypeSymbol arrayTypeSymbol)
                {
                    return nameof(Enumerable.ToArray);
                }

                if ((targetPropertyType as INamedTypeSymbol).ConstructUnboundGenericType()
                    .SafeEquals(typeof(IQueryable<>)))
                {
                    return nameof(Queryable.AsQueryable);
                }

                return nameof(Enumerable.ToList);
            }
        }

        private bool CanAssign(ITypeSymbol source, ITypeSymbol target, Compilation compilation)
        {
            var conversion = compilation.ClassifyConversion(source, target);
            return conversion.IsImplicit || conversion.IsBoxing;
        }
        private bool CanAssign(ITypeSymbol source, ITypeSymbol target, MapperContext context)
        {
            return CanAssign(source, target, context.Compilation);
        }

        private string FormatRefrence(string refrenceName, string expression)
        {
            if (string.IsNullOrEmpty(refrenceName))
            {
                return expression;
            }

            return $"{refrenceName}.{expression}";
        }


        static ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>
            sourcePropertiesCache = new ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>();
        private Dictionary<string, IPropertySymbol> GetSourcePropertyDictionary(ITypeSymbol typeSymbol)
        {
            return sourcePropertiesCache.GetOrAdd(typeSymbol, (s) => s.GetAllMembers()
                 .OfType<IPropertySymbol>()
                 .Where(p => !p.IsWriteOnly && p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer && p.DeclaredAccessibility == Accessibility.Public)
                 .Select(p => new { p.Name, Property = p })
                 .ToLookup(p => p.Name)
                 .ToDictionary(p => p.Key, p => p.First().Property));
        }
        static ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>
           targetPropertiesCache = new ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>();

        private Dictionary<string, IPropertySymbol> GetTargetPropertyDictionary(ITypeSymbol typeSymbol)
        {
            return targetPropertiesCache.GetOrAdd(typeSymbol, s => s.GetAllMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer && p.DeclaredAccessibility == Accessibility.Public)
                .Select(p => new { p.Name, Property = p })
                .ToLookup(p => p.Name)
                .ToDictionary(p => p.Key, p => p.First().Property));
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
                .Except(mappingInfo.IgnoreTargetProperties).Except(mappingInfo.CustomerMappings.Keys).ToList();

            var targetNotFilledKeys = targetProps.Keys.Except(mappingInfo.IgnoreTargetProperties)
                .Except(mappingInfo.CustomerMappings.Keys).Except(mappingPropKeys).ToList();

            var sourceNotUsedKeys = sourceProps.Keys.Except(mappingInfo.IgnoreTargetProperties)
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
                    this.ReportReadOnlyPropertyCanNotFilled(convertContext, targetProp, sourceProp);
                    continue;
                }

                if (CanCopyingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection copy
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    AppendCollectionCopyAssign(sourceRefrenceName, targetRefrenceName, propName, propName, newConvertContext);
                    queue.AddCollectionCopyMethod(newConvertContext);

                }
                else if (CanMappingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection new object
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingCollectionProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, propName,
                        lineSplitChar);
                }
                else if (CanCopyingSubObjectProperty(sourcePropType, targetPropType, convertContext))
                {
                    // sub object copy
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    AppendSubObjectCopyAssign(sourceRefrenceName, targetRefrenceName, propName, propName, newConvertContext);
                    queue.AddObjectCopyMethod(newConvertContext);
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
                    MappingSubObjectProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, propName,
                        lineSplitChar);
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
        private void AppendSubObjectCopyAssign(string sourceRefrenceName, string targetRefrenceName, string sourcePropName, string targetPropName, MapperContext newConvertContext)
        {
            var codeBuilder = newConvertContext.CodeBuilder;
            var mappingInfo = newConvertContext.MappingInfo;
            if (!mappingInfo.SourceType.IsValueType)
            {
                codeBuilder.AppendCodeLines($"if ({FormatRefrence(sourceRefrenceName, sourcePropName)} == null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"{FormatRefrence(targetRefrenceName, targetPropName)} = default;");
                codeBuilder.EndSegment();
                codeBuilder.AppendCodeLines("else");
                codeBuilder.BeginSegment();
                if (mappingInfo.TargetType.IsValueType)
                {
                    codeBuilder.AppendCodeLines($"{BuildCopyObjectMethodName(mappingInfo.SourceType, mappingInfo.TargetType)}({FormatRefrence(sourceRefrenceName, sourcePropName)}, ref {FormatRefrence(targetRefrenceName, targetPropName)})");
                }
                else
                {
                    codeBuilder.AppendCodeLines($"if ({FormatRefrence(targetRefrenceName, targetPropName)} == null)");
                    codeBuilder.BeginSegment();
                    //创建新对象
                    MappingSubObjectProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, sourcePropName, ";");
                    codeBuilder.EndSegment();
                    codeBuilder.AppendCodeLines("else");
                    codeBuilder.BeginSegment();
                    //复制
                    codeBuilder.AppendCodeLines($"{BuildCopyObjectMethodName(mappingInfo.SourceType, mappingInfo.TargetType)}({FormatRefrence(sourceRefrenceName, sourcePropName)}, {FormatRefrence(targetRefrenceName, targetPropName)});");
                    codeBuilder.EndSegment();
                }
                codeBuilder.EndSegment();
            }
            else
            {
                if (mappingInfo.TargetType.IsValueType)
                {
                    codeBuilder.AppendCodeLines($"{BuildCopyObjectMethodName(mappingInfo.SourceType, mappingInfo.TargetType)}({FormatRefrence(sourceRefrenceName, sourcePropName)}, ref {FormatRefrence(targetRefrenceName, targetPropName)});");
                }
                else
                {
                    codeBuilder.AppendCodeLines($"if ({FormatRefrence(targetRefrenceName, targetPropName)} == null)");
                    codeBuilder.BeginSegment();
                    //创建新的对象
                    MappingSubObjectProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, sourcePropName, ";");
                    codeBuilder.EndSegment();
                    codeBuilder.AppendCodeLines("else");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"{BuildCopyObjectMethodName(mappingInfo.SourceType, mappingInfo.TargetType)}({FormatRefrence(sourceRefrenceName, sourcePropName)}, {FormatRefrence(targetRefrenceName, targetPropName)});");
                    codeBuilder.EndSegment();
                }
            }
        }
        private void AppendCollectionCopyAssign(string sourceRefrenceName, string targetRefrenceName, string sourcePropName, string targetPropName, MapperContext newConvertContext)
        {
            var mappingInfo = newConvertContext.MappingInfo;
            var codeBuilder = newConvertContext.CodeBuilder;
            codeBuilder.AppendCodeLines($"if ({FormatRefrence(sourceRefrenceName, sourcePropName)} == null || {FormatRefrence(targetRefrenceName, targetPropName)} == null)");
            codeBuilder.BeginSegment();
            //直接给集合赋值
            MappingCollectionProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, targetPropName,
                            ";");
            codeBuilder.EndSegment();
            codeBuilder.AppendCodeLines("else");
            codeBuilder.BeginSegment();
            codeBuilder.AppendCodeLines($"{BuildCopyCollectionMethodName(mappingInfo.SourceType, mappingInfo.TargetType)}({FormatRefrence(sourceRefrenceName, sourcePropName)}, {FormatRefrence(targetRefrenceName, targetPropName)});");
            codeBuilder.EndSegment();

        }

        private void AppendPropertyAssign(string sourceRefrenceName, string targetRefrenceName, string lineSplitChar,
            MapperContext convertContext)
        {
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
                .Except(mappingInfo.IgnoreTargetProperties).Except(mappingInfo.CustomerMappings.Keys).ToList();

            var targetNotFilledKeys = targetProps.Keys.Except(mappingInfo.IgnoreTargetProperties)
                .Except(mappingInfo.CustomerMappings.Keys).Except(mappingPropKeys).ToList();

            var sourceNotUsedKeys = sourceProps.Keys.Except(mappingInfo.IgnoreTargetProperties)
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
                    this.ReportReadOnlyPropertyCanNotFilled(convertContext, targetProp, sourceProp);
                    continue;
                }

                if (CanAssign(sourcePropType, targetPropType, convertContext))
                {
                    // default 
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, propName)} = {FormatRefrence(sourceRefrenceName, propName)}{lineSplitChar}");
                }
                else if (CanMappingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingCollectionProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, propName,
                        lineSplitChar);
                }
                else if (CanMappingSubObjectProperty(sourcePropType, targetPropType, convertContext))
                {
                    // sub object 
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingSubObjectProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, propName,
                        lineSplitChar);
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
        private void ReportReadOnlyPropertyCanNotFilled(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {
            var mappingInfo = context.MappingInfo;
            context.CodeWriter.Context.ReportTargetPropertyNotFilledBecauseIsGetOnly(targetProperty, mappingInfo.SourceType, mappingInfo.TargetType);

        }
        private void ReportTargetPrpertyNotFilled(MapperContext context, IPropertySymbol targetProperty)
        {
            if (context.MappingInfo.CheckTargetPropertiesFullFilled)
            {
                context.CodeWriter.Context.ReportTargetPropertyNotFilled(targetProperty, context.MappingInfo.SourceType, context.MappingInfo.TargetType);
            }
        }
        private void ReportSourcePropertyNotMapped(MapperContext context, IPropertySymbol sourceProperty)
        {
            if (context.MappingInfo.CheckSourcePropertiesFullUsed)
            {
                context.CodeWriter.Context.ReportSourcePropertyNotMapped(sourceProperty, context.MappingInfo.SourceType, context.MappingInfo.TargetType);
            }
        }
        private void ReportPropertyCanNotBeMapped(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {

        }
        private void ReportError(string type, MapperContext context, IDictionary<string, IPropertySymbol> sourceProperties, IDictionary<string, IPropertySymbol> targetProperties, IList<IPropertySymbol> usedSourceProperties)
        {
            var mappingInfo = context.MappingInfo;
            if (mappingInfo.CheckSourcePropertiesFullUsed)
            {
                foreach (var source in sourceProperties)
                {
                    if (usedSourceProperties.Contains(source.Value))
                    {
                        continue;
                    }
                    if (mappingInfo.IgnoreTargetProperties.Contains(source.Key))
                    {
                        continue;
                    }
                    //report
                    context.CodeWriter.Context.ReportSourcePropertyNotMapped(source.Value, mappingInfo.SourceType, mappingInfo.TargetType);
                }
            }
        }
        bool FindNavigatePaths(string targetName,
             Dictionary<string, IPropertySymbol> sourceProperties, ref List<KeyValuePair<string, IPropertySymbol>> paths)
        {
            var comparisonRule = StringComparison.InvariantCultureIgnoreCase;
            if (string.IsNullOrEmpty(targetName))
            {
                return false;
            }

            var matchProps = sourceProperties
                .Where(p => targetName.StartsWith(p.Key, comparisonRule))
                .OrderByDescending(p => p.Key.Length);


            foreach (var matchProp in matchProps)
            {
                if (matchProp.Key.Equals(targetName, comparisonRule))
                {
                    paths.Add(matchProp);
                    return true;
                }
                else
                {
                    var leftName = targetName.Substring(matchProp.Key.Length);
                    if (FindNavigatePaths(leftName, GetSourcePropertyDictionary(matchProp.Value.Type), ref paths))
                    {
                        paths.Insert(0, matchProp);
                        return true;
                    }
                }
            }
            return false;
        }

        bool HasSuggestionPath(KeyValuePair<string, IPropertySymbol> target,
            Dictionary<string, IPropertySymbol> sourceProperties, out List<IPropertySymbol> paths, MapperContext context)
        {
            var navPaths = new List<KeyValuePair<string, IPropertySymbol>>();
            if (FindNavigatePaths(target.Key, sourceProperties, ref navPaths))
            {
                if (CanAssign(navPaths.Last().Value.Type, target.Value.Type, context))
                {
                    paths = navPaths.Select(p => p.Value).ToList();
                    return true;
                }
            }
            paths = navPaths.Select(p => p.Value).ToList();
            return false;
        }
    }
}
