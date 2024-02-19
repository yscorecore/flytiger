using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using static FlyTiger.Mapper.Generators.Utils;

namespace FlyTiger.Mapper.Generators
{
    internal class ConvertObjectGenerator : BaseGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;

            if (!mappingInfo.MapConvert) return;
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
        protected void AppendPropertyAssign(string sourceRefrenceName, string targetRefrenceName, string lineSplitChar,
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

                if (CanAssign(sourcePropType, targetPropType, convertContext))
                {
                    // default 
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, propName)} = {FormatRefrence(sourceRefrenceName, propName)}{lineSplitChar}");
                }
                else if (CanMappingDictionary(convertContext, targetProp, sourceProp))
                {
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingNewDictionary(newConvertContext, FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), lineSplitChar);
                    // dictionary
                }
                else if (CanMappingCollectionProperty(sourcePropType, targetPropType, convertContext))
                {
                    // collection
                    var newConvertContext = convertContext.Fork(sourcePropType, targetPropType);
                    MappingNewCollection(newConvertContext, FormatRefrence(sourceRefrenceName, propName), FormatRefrence(targetRefrenceName, propName), lineSplitChar);
                }
                else if (CanMappingSubObjectProperty(sourcePropType, targetPropType, convertContext))
                {
                    // sub object 
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

        protected bool CanMappingDictionary(MapperContext context, IPropertySymbol targetProperty, IPropertySymbol sourceProperty)
        {
            if (IsDictionary(targetProperty.Type) && IsDictionary(sourceProperty.Type))
            {
                var (sKey, sValue) = GetKeyValueType(sourceProperty.Type as INamedTypeSymbol);
                var (tKey, tValue) = GetKeyValueType(targetProperty.Type as INamedTypeSymbol);
                if (CanAssign(sKey, tKey, context) || CanMappingSubObjectProperty(sKey, tKey, context))
                {
                    return CanAssign(sValue, tValue, context) || CanMappingSubObjectProperty(sValue, tValue, context);
                }
            }

            return false;


            (ITypeSymbol, ITypeSymbol) GetKeyValueType(INamedTypeSymbol type)
            {
                return (type.TypeArguments[0], type.TypeArguments[1]);
            }
        }
        protected void MappingNewDictionary(MapperContext context, string sourceRefrenceExpression, string targetRefrenceExpression, string lineSplitChar)
        {
            var (sKey, sValue) = GetKeyValueType(context.MappingInfo.SourceType as INamedTypeSymbol);
            var (tKey, tValue) = GetKeyValueType(context.MappingInfo.TargetType as INamedTypeSymbol);
            var methodName = ToTargetMethodName();


            context.CodeBuilder.AppendCodeLines($"{targetRefrenceExpression} = {sourceRefrenceExpression} == null ? default : {sourceRefrenceExpression}.{methodName}(");
            context.CodeBuilder.IncreaseDepth();
            AppendLambda(context.CodeBuilder, nameof(KeyValuePair<int, int>.Key), tKey, sKey, ",");
            AppendLambda(context.CodeBuilder, nameof(KeyValuePair<int, int>.Value), tValue, sValue, $"){lineSplitChar}");
            context.CodeBuilder.DecreaseDepth();
            string ToTargetMethodName()
            {
                var arg = (context.MappingInfo.TargetType as INamedTypeSymbol).ConstructUnboundGenericType();
                if (arg.SafeEquals(typeof(IImmutableDictionary<,>)) || arg.SafeEquals(typeof(ImmutableDictionary<,>)))
                {
                    return nameof(ImmutableDictionary.ToImmutableDictionary);
                }
                return nameof(Enumerable.ToDictionary);
            }

            (ITypeSymbol, ITypeSymbol) GetKeyValueType(INamedTypeSymbol type)
            {
                return (type.TypeArguments[0], type.TypeArguments[1]);
            }

            void AppendLambda(CsharpCodeBuilder codeBuilder, string name, ITypeSymbol targetType, ITypeSymbol sourceType, string tail)
            {
                if (targetType.SafeEquals(sourceType))
                {
                    codeBuilder.AppendCodeLines($"p => p.{name}{tail}");
                }
                else if (CanAssign(sourceType, targetType, context))
                {
                    codeBuilder.AppendCodeLines($"p => ({targetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})p.{name}{tail}");
                }
                else
                {
                    var newContext = context.Fork(sourceType, targetType);
                    MappingNewSubObject(newContext, $"p.{name}", "p", "=>", tail);
                }
            }
        }

        protected void MappingNewSubObject(MapperContext convertContext, string sourceRefrenceName, string targetRefrenceName, string assignCode, string tail)
        {
            var targetPropertyType = convertContext.MappingInfo.TargetType;
            var sourcePropertyType = convertContext.MappingInfo.SourceType;

            var sourceIsNullable = sourcePropertyType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
            var targetIsNullable = targetPropertyType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

            var sourceActualType = sourceIsNullable ? ((INamedTypeSymbol)sourcePropertyType).TypeArguments.First() : sourcePropertyType;
            var targetActualType = targetIsNullable ? ((INamedTypeSymbol)targetPropertyType).TypeArguments.First() : targetPropertyType;

            var context = (sourceIsNullable || targetIsNullable) ? convertContext.Fork(sourceActualType, targetActualType) : convertContext;
            var codeBuilder = convertContext.CodeBuilder;
            var targetPropertyTypeText = targetActualType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (!sourceIsNullable && sourceActualType.IsValueType)
            {
                if (sourcePropertyType.NullableAnnotation != NullableAnnotation.Annotated)
                {
                    codeBuilder.AppendCodeLines($"{targetRefrenceName} {assignCode} new {targetPropertyTypeText}");
                }
            }
            else
            {
                codeBuilder.AppendCodeLines(
                    $"{targetRefrenceName} {assignCode} {sourceRefrenceName} == null ? default : new {targetPropertyTypeText}");

            }
            codeBuilder.BeginSegment();
            if (sourceIsNullable)
            {
                AppendPropertyAssign(FormatRefrence(sourceRefrenceName, nameof(Nullable<int>.Value)), null, ",", context);
            }
            else
            {
                AppendPropertyAssign(sourceRefrenceName, null, ",", context);
            }
            codeBuilder.EndSegment("}" + tail);
        }
        protected bool CanMappingCollectionProperty(ITypeSymbol sourcePropType, ITypeSymbol targetPropType,
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
                            targetUnboundGenericType.SafeEquals(typeof(ICollection<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(IImmutableList<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(ImmutableList<>)) ||
                            targetUnboundGenericType.SafeEquals(typeof(ImmutableArray<>));
                    }
                }

                return false;
            }
        }


        protected void MappingNewCollection(MapperContext convertContext, string sourcePropertyExpression, string targetPropertyExpression, string lineSplitChar)
        {
            var codeBuilder = convertContext.CodeBuilder;
            var targetPropertyType = convertContext.MappingInfo.TargetType;
            var sourcePropertyType = convertContext.MappingInfo.SourceType;
            var targetItemType = GetItemType(targetPropertyType);
            var sourceItemType = GetItemType(sourcePropertyType);
            var targetItemTypeText = targetItemType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);


            if (sourceItemType.SafeEquals(targetItemType))
            {
                codeBuilder.AppendCodeLines(
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default : {sourcePropertyExpression}.{ToTargetMethodName()}(){lineSplitChar}");
            }
            else if (CanAssign(sourceItemType, targetItemType, convertContext))
            {
                codeBuilder.AppendCodeLines(
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default : {sourcePropertyExpression}.Select(p => ({targetItemTypeText})p).{ToTargetMethodName()}(){lineSplitChar}");
            }
            else
            {
                if (sourceItemType.IsValueType)
                {
                    codeBuilder.AppendCodeLines(
                        $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default : {sourcePropertyExpression}.Select(p => new {targetItemTypeText}");
                }
                else
                {
                    codeBuilder.AppendCodeLines(
                        $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default : {sourcePropertyExpression}.Select(p => p == null ? default({targetItemTypeText}) : new {targetItemTypeText}");
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

                if (targetPropertyType is INamedTypeSymbol namedType)
                {
                    var genericType = namedType.ConstructUnboundGenericType();
                    if (genericType.SafeEquals(typeof(IQueryable<>)))
                    {
                        return nameof(Queryable.AsQueryable);
                    }
                    if (genericType.SafeEquals(typeof(ImmutableArray<>)))
                    {
                        return nameof(ImmutableArray.ToImmutableArray);
                    }
                    if (genericType.SafeEquals(typeof(IImmutableList<>)))
                    {
                        return nameof(ImmutableList.ToImmutableList);
                    }
                    if (genericType.SafeEquals(typeof(ImmutableList<>)))
                    {
                        return nameof(ImmutableList.ToImmutableList);
                    }
                }

                return nameof(Enumerable.ToList);

            }
        }




    }
}
