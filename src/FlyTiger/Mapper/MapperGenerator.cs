using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger
{
    [Generator]
    class MapperGenerator : ISourceGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        const string AttributeName = "MapperAttribute";
        const string SourceTypePropertyName = "SourceType";
        const string TargetTypePropertyName = "TargetType";
        const string IgnoreTargetPropertiesPropertyName = "IgnoreTargetProperties";
        const string CustomMappingsPropertyName = "CustomMappings";
        internal static string AttributeFullName = $"{NameSpaceName}.{AttributeName}";

        const string AttributeCode = @"using System;
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class MapperAttribute : Attribute
    {
        public MapperAttribute(Type sourceType, Type targetType)
        {

            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
        }

        public Type SourceType { get; }
        public Type TargetType { get; }

        public string[] IgnoreTargetProperties { get; set; }

        public string[] CustomMappings { get; set; }

    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization((i) =>
            {
                i.AddSource($"{AttributeFullName}.g.cs", AttributeCode);
            });
            context.RegisterForSyntaxNotifications(() => new MapperSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is MapperSyntaxReceiver receiver))
                return;
            var codeWriter = new CodeWriter(context);

            var attributes = FindAllAttributes(codeWriter, receiver.CandidateClasses);
            if (attributes.Count > 0)
            {
                var codeFile = new CodeFile
                {
                    BasicName = "MapperExtensions",
                    Content = BuildMapperContent(codeWriter, attributes),
                };
                codeWriter.WriteCodeFile(codeFile);

            }
        }

        private string BuildMapperContent(CodeWriter codeWriter, IList<AttributeData> attributeDatas)
        {
            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder();
            this.AppendUsingLines(codeBuilder);
            this.AppendNamespace(codeBuilder);
            this.AppendClassDefinition(codeBuilder);
            this.AppendCommonFunctions(codeBuilder);
            this.AppendAllConvertFunctions(codeWriter, codeBuilder, attributeDatas);
            this.AppendAllGenericFunctions(codeWriter, codeBuilder, attributeDatas);
            codeBuilder.EndAllSegments();
            return codeBuilder.ToString();
        }
        private IList<AttributeData> FindAllAttributes(CodeWriter codeWriter, IList<ClassDeclarationSyntax> candidateClasses)
        {
            return codeWriter.GetAllClassSymbolsIgnoreRepeated(candidateClasses)
                  .SelectMany(p => p.GetAttributes().Where(t => t.AttributeClass.Is(AttributeFullName)))
                  .ToList();
        }




        private void AppendAllConvertFunctions(CodeWriter codeWriter,
            CsharpCodeBuilder codeBuilder, IList<AttributeData> attributeDatas)
        {
            //System.Diagnostics.Debugger.Launch();
            foreach (var convertToAttr in attributeDatas)
            {
                var convertMappingInfo = ConvertMappingInfo.FromAttributeData(convertToAttr);
                if (ConvertMappingInfo.CanMappingSubObject(convertMappingInfo.SourceType,
                        convertMappingInfo.TargetType))
                {
                    AppendConvertToFunctions(new MapperContext(codeWriter.Compilation, codeBuilder, convertMappingInfo, attributeDatas));
                }
                else
                {
                    // TOTO report error
                }
            }
        }

        private void AppendAllGenericFunctions(CodeWriter codeWriter,
            CsharpCodeBuilder codeBuilder, IList<AttributeData> attributeDatas)
        {
            var allSources = attributeDatas.Where(p => p.AttributeClass.Is(AttributeFullName))
                .Select(p => ConvertMappingInfo.FromAttributeData(p))
                .ToLookup(p => p.SourceType);
            foreach (var item in allSources)
            {
                AppendGenericFunctions(item.Key, item.ToList(), codeBuilder);
            }
        }
        private void AppendCommonFunctions(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines(@"private static IEnumerable<T> EachItem<T>(this IEnumerable<T> source, Action<T> handler)
{
    foreach (var item in source)
    {
        handler(item);
        yield return item;
    }
}");
        }

        private void AppendGenericFunctions(ITypeSymbol fromType, List<ConvertMappingInfo> mappingInfos,
            CsharpCodeBuilder codeBuilder)
        {
            var methodName = "To";
            var fromTypeDisplay = fromType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            AddToMethodForSingle();
            AddToMethodForSingleWithPostAction();
            AddCopyToMethodForSingle();
            AddToMethodForEnumable();
            AddToMethodForEnumableWithPostAction();
            AddToMethodForQueryable();

            void AddToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static T {methodName}<T>(this {fromTypeDisplay} source) where T:new()");
                codeBuilder.BeginSegment();
                if (!fromType.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return default;");
                }

                foreach (var mapping in mappingInfos)
                {
                    var toTypeDisplay = mapping.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({toTypeDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (T)(object){mapping.ConvertToMethodName}(source);");
                    codeBuilder.EndSegment();
                }

                codeBuilder.AppendCodeLines(
                    $"throw new NotSupportedException($\"Can not convert '{{typeof({fromTypeDisplay})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
            void AddToMethodForSingleWithPostAction()
            {
                codeBuilder.AppendCodeLines(
                   $"public static T {methodName}<T>(this {fromTypeDisplay} source, Action<T> postHandler) where T:new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($"var result = source.{methodName}<T>();");
                codeBuilder.AppendCodeLines("postHandler?.Invoke(result);");
                codeBuilder.AppendCodeLines("return result;");
                codeBuilder.EndSegment();
            }

            void AddCopyToMethodForSingle()
            {
                codeBuilder.AppendCodeLines(
                    $"public static void {methodName}<T>(this {fromTypeDisplay} source, T target) where T:class");
                codeBuilder.BeginSegment();
                if (!fromType.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return;");
                }

                foreach (var mapping in mappingInfos)
                {
                    var toTypeDisplay = mapping.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({toTypeDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines(
                        $"{mapping.ConvertToMethodName}(source, ({toTypeDisplay})(object)target);");
                    codeBuilder.AppendCodeLines($"return;");
                    codeBuilder.EndSegment();
                }

                codeBuilder.AppendCodeLines(
                    $"throw new NotSupportedException($\"Can not convert '{{typeof({fromTypeDisplay})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }

            void AddToMethodForEnumable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromTypeDisplay}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    var toTypeDisplay = mapping.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({toTypeDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (IEnumerable<T>)source?.Select(p => p.{mapping.ConvertToMethodName}());");
                    codeBuilder.EndSegment();
                }

                codeBuilder.AppendCodeLines(
                    $"throw new NotSupportedException($\"Can not convert '{{typeof({fromTypeDisplay})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
            void AddToMethodForEnumableWithPostAction()
            {

                codeBuilder.AppendCodeLines($"public static IEnumerable<T> {methodName}<T>(this IEnumerable<{fromTypeDisplay}> source, Action<T> postHandler) where T : new()");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines("return source == null || postHandler == null ? source.To<T>() : source.To<T>().EachItem(postHandler);");
                codeBuilder.EndSegment();
            }

            void AddToMethodForQueryable()
            {
                codeBuilder.AppendCodeLines(
                    $"public static IQueryable<T> {methodName}<T>(this IQueryable<{fromTypeDisplay}> source) where T:new()");
                codeBuilder.BeginSegment();
                foreach (var mapping in mappingInfos)
                {
                    var toTypeDisplay = mapping.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    codeBuilder.AppendCodeLines($"if (typeof(T) == typeof({toTypeDisplay}))");
                    codeBuilder.BeginSegment();
                    codeBuilder.AppendCodeLines($"return (IQueryable<T>){mapping.ConvertToMethodName}(source);");
                    codeBuilder.EndSegment();
                }

                codeBuilder.AppendCodeLines(
                    $"throw new NotSupportedException($\"Can not convert '{{typeof({fromTypeDisplay})}}' to '{{typeof(T)}}'.\");");
                codeBuilder.EndSegment();
            }
        }

        private void AppendUsingLines(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines("using System;");
            codeBuilder.AppendCodeLines("using System.Collections.Generic;");
            codeBuilder.AppendCodeLines("using System.Linq;");
        }

        private void AppendNamespace(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines($"namespace FlyTiger");
            codeBuilder.BeginSegment();
        }

        private void AppendClassDefinition(CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines($@"static class MapperExtensions");
            codeBuilder.BeginSegment();
        }

        private void AppendConvertToFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var toType = mappingInfo.TargetType;
            var fromType = mappingInfo.SourceType;
            var toTypeDisplay = mappingInfo.TargetType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var fromTypeDisplay = mappingInfo.SourceType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            AddToMethodForSingle();
            AddCopyToMethodForSingle();
            AddToMethodForQueryable();

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
                    $"private static void {mappingInfo.ConvertToMethodName}(this {fromTypeDisplay} source, {toTypeDisplay} target)");
                codeBuilder.BeginSegment();
                if (!fromType.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (source == null) return;");
                }

                if (!toType.IsValueType)
                {
                    codeBuilder.AppendCodeLines("if (target == null) return;");
                }

                AppendPropertyAssign("source", "target", ";", context);
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
                codeBuilder.EndSegment("});");
                codeBuilder.EndSegment();
            }
        }

        private bool CanMappingSubObjectProperty(ITypeSymbol sourceType, ITypeSymbol targetType,
            MapperContext convertContext)
        {
            if (sourceType.IsPrimitive() || targetType.IsPrimitive())
            {
                return false;
            }

            if (convertContext.HasWalked(targetType))
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

        private ITypeSymbol GetItemType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
            {
                return arrayTypeSymbol.ElementType;
            }

            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                return namedTypeSymbol.TypeArguments[0];
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
                    $"{targetPropertyExpression} = {sourcePropertyExpression} == null ? default({targetPropertyTypeText}): new {targetPropertyTypeText}");
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

        private Dictionary<string, ITypeSymbol> GetSourcePropertyDictionary(ITypeSymbol typeSymbol)
        {
            // TODO use cache
            return typeSymbol.GetAllMembers()
                .OfType<IPropertySymbol>()
                .Where(p => !p.IsWriteOnly && p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer)
                .Select(p => new { p.Name, Type = p.Type })
                .ToLookup(p => p.Name)
                .ToDictionary(p => p.Key, p => p.First().Type);
        }
        private Dictionary<string, ITypeSymbol> GetTargetPropertyDictionary(ITypeSymbol typeSymbol)
        {
            // TODO use cache
            return typeSymbol.GetAllMembers()
                .OfType<IPropertySymbol>()
                .Where(p => !p.IsReadOnly && p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer)
                .Select(p => new { p.Name, Type = p.Type })
                .ToLookup(p => p.Name)
                .ToDictionary(p => p.Key, p => p.First().Type);
        }

        private void AppendPropertyAssign(string sourceRefrenceName, string targetRefrenceName, string lineSplitChar,
            MapperContext convertContext)
        {
            var mappingInfo = convertContext.MappingInfo;
            var codeBuilder = convertContext.CodeBuilder;
            var targetProps = GetTargetPropertyDictionary(mappingInfo.TargetType);
            var sourceProps = GetSourcePropertyDictionary(mappingInfo.SourceType);
            foreach (var prop in targetProps)
            {
                if (mappingInfo.IgnoreTargetProperties != null && mappingInfo.IgnoreTargetProperties.Contains(prop.Key))
                {
                    continue;
                }

                if (mappingInfo.CustomerMappings != null &&
                    mappingInfo.CustomerMappings.TryGetValue(prop.Key, out var sourceExpression))
                {
                    var actualSourceExpression = sourceExpression.Replace("$", sourceRefrenceName);
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, prop.Key)} = {actualSourceExpression}{lineSplitChar}");
                }
                else if (sourceProps.TryGetValue(prop.Key, out var sourcePropType))
                {
                    if (CanAssign(sourcePropType, prop.Value, convertContext))
                    {
                        // default 
                        codeBuilder.AppendCodeLines(
                            $"{FormatRefrence(targetRefrenceName, prop.Key)} = {FormatRefrence(sourceRefrenceName, prop.Key)}{lineSplitChar}");
                    }
                    else if (CanMappingCollectionProperty(sourcePropType, prop.Value, convertContext))
                    {
                        // collection
                        var newConvertContext = convertContext.Fork(sourcePropType, prop.Value);
                        MappingCollectionProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, prop.Key,
                            lineSplitChar);
                    }
                    else if (CanMappingSubObjectProperty(sourcePropType, prop.Value, convertContext))
                    {
                        // sub object 
                        var newConvertContext = convertContext.Fork(sourcePropType, prop.Value);
                        MappingSubObjectProperty(newConvertContext, sourceRefrenceName, targetRefrenceName, prop.Key,
                            lineSplitChar);
                    }
                }
                // eg UserName = User.Name
                else if (HasSuggestionPath(prop, sourceProps, out var paths))
                {
                    var actualSourceExpression = $"{sourceRefrenceName}.{string.Join(".", paths)}";
                    codeBuilder.AppendCodeLines(
                        $"{FormatRefrence(targetRefrenceName, prop.Key)} = {actualSourceExpression}{lineSplitChar}");
                }
            }

            bool FindNavigatePaths(string targetName,
                Dictionary<string, ITypeSymbol> sourceProperties, ref List<KeyValuePair<string, ITypeSymbol>> paths)
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
                        if (FindNavigatePaths(leftName, GetSourcePropertyDictionary(matchProp.Value), ref paths))
                        {
                            paths.Insert(0, matchProp);
                            return true;
                        }
                    }
                }
                return false;
            }

            bool HasSuggestionPath(KeyValuePair<string, ITypeSymbol> target,
                Dictionary<string, ITypeSymbol> sourceProperties, out List<string> paths)
            {
                var navPaths = new List<KeyValuePair<string, ITypeSymbol>>();
                if (FindNavigatePaths(target.Key, sourceProperties, ref navPaths))
                {
                    if (CanAssign(navPaths.Last().Value, target.Value, convertContext))
                    {
                        paths = navPaths.Select(p => p.Key).ToList();
                        return true;
                    }
                }
                paths = navPaths.Select(p => p.Key).ToList();
                return false;
            }
        }

        private class MapperSyntaxReceiver : ISyntaxReceiver
        {
            public IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                    classDeclarationSyntax.AttributeLists.Any())
                {
                    CandidateClasses.Add(classDeclarationSyntax);
                }
            }
        }

        private class MapperContext
        {
            public MapperContext(Compilation compilation, CsharpCodeBuilder codeBuilder,
                ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
            {
                this.Compilation = compilation;
                this.MappingInfo = convertMappingInfo;
                this.CodeBuilder = codeBuilder;
                this.AttributeLists = attributeList;
                this.WorkedPaths.Add(convertMappingInfo.TargetType);
            }

            private MapperContext(MapperContext baseConvertContext, ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
                : this(baseConvertContext.Compilation, baseConvertContext.CodeBuilder,
                    convertMappingInfo, attributeList)
            {
                this.AttributeLists = this.AttributeLists;
                this.WorkedPaths = new List<ITypeSymbol>(baseConvertContext.WorkedPaths);
                this.WorkedPaths.Add(convertMappingInfo.TargetType);
            }

            public Compilation Compilation { get; }
            public ConvertMappingInfo MappingInfo { get; }
            public CsharpCodeBuilder CodeBuilder { get; }
            public List<ITypeSymbol> WorkedPaths { get; } = new List<ITypeSymbol>();
            public IList<AttributeData> AttributeLists { get; } = new List<AttributeData>();
            public bool HasWalked(ITypeSymbol symbol)
            {
                foreach (var path in WorkedPaths)
                {
                    if (path.Equals(symbol, SymbolEqualityComparer.Default))
                    {
                        return true;
                    }
                }

                return false;
            }

            public MapperContext Fork(ITypeSymbol source, ITypeSymbol target)
            {


                var newMappingInfo = CreateMappingInfo();
                return new MapperContext(this, newMappingInfo, this.AttributeLists);

                ConvertMappingInfo CreateMappingInfo()
                {
                    var attributeData = this.AttributeLists
                   .Where(p => (p.ConstructorArguments.First().Value as INamedTypeSymbol).Equals(source,
                       SymbolEqualityComparer.Default))
                   .Where(p => (p.ConstructorArguments.Last().Value as INamedTypeSymbol).Equals(target,
                       SymbolEqualityComparer.Default))
                   .FirstOrDefault();
                    if (attributeData != null)
                    {
                        return ConvertMappingInfo.FromAttributeData(attributeData);
                    }
                    else
                    {
                        return new ConvertMappingInfo
                        {
                            SourceType = source,
                            TargetType = target,
                            CustomerMappings = new Dictionary<string, string>(),
                            IgnoreTargetProperties = new HashSet<string>()
                        };
                    }
                }
            }
        }

        private class ConvertMappingInfo
        {
            public ITypeSymbol TargetType { get; internal set; }
            public ITypeSymbol SourceType { get; internal set; }
            public HashSet<string> IgnoreTargetProperties { get; internal set; }
            public Dictionary<string, string> CustomerMappings { get; internal set; }
            public string ConvertToMethodName { get; internal set; }

            public static ConvertMappingInfo FromAttributeData(AttributeData attributeData)
            {
                var arguments = attributeData.ConstructorArguments;
                var fromType = arguments.First().Value as INamedTypeSymbol;
                var toType = arguments.Last().Value as INamedTypeSymbol;
                var ignoreProperties = attributeData.NamedArguments
                    .Where(p => p.Key == IgnoreTargetPropertiesPropertyName)
                    .Where(p => p.Value.IsNull == false)
                    .SelectMany(p => p.Value.Values.Select(t => (string)t.Value))
                    .Where(p => !string.IsNullOrWhiteSpace(p));
                var customMappings = attributeData.NamedArguments
                    .Where(p => p.Key == CustomMappingsPropertyName)
                    .Where(p => p.Value.IsNull == false)
                    .SelectMany(p => p.Value.Values.Select(t => (string)t.Value))
                    .Select(ParseCustomMapping)
                    .Where(item => item != null)
                    .Select(item => item.Value)
                    .ToLookup(item => item.Key, item => item.Value)
                    .ToDictionary(p => p.Key, p => p.Last());
                var methodName = new string(toType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                    .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
                return new ConvertMappingInfo
                {
                    SourceType = fromType,
                    TargetType = toType,
                    IgnoreTargetProperties = new HashSet<string>(ignoreProperties),
                    CustomerMappings = customMappings,
                    ConvertToMethodName = $"To{methodName}",
                };
            }


            static KeyValuePair<string, string>? ParseCustomMapping(string expression)
            {
                if (string.IsNullOrWhiteSpace(expression))
                {
                    return null;
                }

                var equalIndex = expression.IndexOf('=');
                if (equalIndex > 0)
                {
                    var targetExpression = expression.Substring(0, equalIndex).Trim();
                    var sourceExpression = expression.Substring(equalIndex + 1).Trim();
                    return new KeyValuePair<string, string>(targetExpression, sourceExpression);
                }
                else
                {
                    return null;
                }
            }

            public static bool CanMappingSubObject(ITypeSymbol sourceType, ITypeSymbol targetType)
            {
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
        }
    }
}
