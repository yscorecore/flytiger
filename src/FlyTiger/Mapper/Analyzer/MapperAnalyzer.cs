using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FlyTiger.Mapper.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static FlyTiger.Mapper.LocationUtils;

namespace FlyTiger.Mapper.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class MapperAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.MapperNotUsed, DiagnosticDescriptors.MissingMapper, DiagnosticDescriptors.DuplicateMapper); }
        }
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            //context.RegisterSyntaxNodeAction(AnalyzeSingleClassNode, SyntaxKind.ClassDeclaration);
            context.RegisterSemanticModelAction(AnalyzeAllTypes);
        }
        internal static void AnalyzeAllTypes(SemanticModelAnalysisContext semanticModelContext)
        {
            SemanticModel semanticModel = semanticModelContext.SemanticModel;
            SyntaxTree syntaxTree = semanticModel.SyntaxTree;
            var root = syntaxTree.GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            AnalyzeAllTypes(semanticModelContext.ReportDiagnostic, classes, semanticModel);
        }


        //private void AnalyzeSingleClassNode(SyntaxNodeAnalysisContext context)
        //{
        //    var classDeclaration = (ClassDeclarationSyntax)context.Node;
        //    var allAttributes = GetAllMapperAttribute(context.SemanticModel, classDeclaration).ToImmutableArray();
        //    var allMethods = GetAllMapperMethodInvoke(context.SemanticModel, classDeclaration).ToImmutableArray();
        //    AnalyzeAttribute(context.ReportDiagnostic, allAttributes, allMethods);
        //}
        private static void AnalyzeAllTypes(Action<Diagnostic> reporter, IEnumerable<ClassDeclarationSyntax> classes, SemanticModel semanticModel)
        {
            var allAttributes = classes.SelectMany(p => GetAllMapperAttribute(semanticModel, p)).ToImmutableArray();
            var allMethos = classes.SelectMany(p => GetAllMapperMethodInvoke(semanticModel, p)).ToImmutableArray();
            AnalyzeAttributes(reporter, allAttributes);
            AnalyzeAttributeAndMethods(reporter, allAttributes, allMethos);
        }
        private static IEnumerable<AttributeDefineInfo> GetAllMapperAttribute(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration)
        {
            var classDefine = semanticModel.GetDeclaredSymbol(classDeclaration);
            return classDefine.GetAttributes().Where(p => p.AttributeClass.Is(MapperGenerator.AttributeFullName))
                .Select(p => new AttributeDefineInfo
                {
                    Attribute = p,
                    SourceLocation = FindAttributeLocation(p),
                    MapperInfo = CreateMapperInfo(p)
                });
           
            MapperInfo CreateMapperInfo(AttributeData att)
            {
                var args = att.ConstructorArguments.ToArray();
                var from = args[0].Value as INamedTypeSymbol;
                var to = args[1].Value as INamedTypeSymbol;
                var mapperType = att.NamedArguments
                    .Where(p => p.Key == MapperGenerator.MapperTypeName)
                    .Where(p => p.Value.IsNull == false)
                    .Select(p => Convert.ToInt32(p.Value.Value))
                    .FirstOrDefault();
                var kind = mapperType == 0 ? Kind.Query | Kind.Update | Kind.Convert : (Kind)mapperType;
                return new MapperInfo { Source = from, Target = to, Kind = kind };
            }
        }
        private static IEnumerable<MethodInvokeInfo> GetAllMapperMethodInvoke(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration)
        {
            var classDefine = semanticModel.GetSymbolInfo(classDeclaration);
            if (classDefine.Symbol != null && classDefine.Symbol.Name == MapperGenerator.MapperExtensionName)
            {
                yield break;
            }
            foreach (var member in classDeclaration.Members)
            {
                foreach (var methodInvocation in member.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (semanticModel.GetSymbolInfo(methodInvocation).Symbol is IMethodSymbol methodSymbol &&
                        methodSymbol.Name == GenericFunctionGenerator.ConvertGenericMethodName &&
                        methodSymbol.ContainingType.Name == MapperGenerator.MapperExtensionName &&
                        methodSymbol.ReceiverType != null)
                    {
                        var mapperInfo = GetMethodMapperInfo(methodSymbol);
                        if (mapperInfo != null)
                        {
                            yield return new MethodInvokeInfo
                            {
                                Method = methodSymbol,
                                MapperInfo = mapperInfo,
                                SourceLocation = methodInvocation.GetLocation()
                            };
                        }

                    }
                }
            }

        }

        private static void AnalyzeAttributes(Action<Diagnostic> reporter, ImmutableArray<AttributeDefineInfo> attributes)
        {
            var allDuplicates = attributes.Select(p => new
            {
                Key = p.MapperInfo.GetMapperKey(),
                Value = p
            }).GroupBy(p => p.Key).Where(t => t.Count() > 1);
            foreach (var g in allDuplicates)
            {
                //优先定义了参数的
                var allMappers = g.Select(p => p.Value).OrderByDescending(p => p.Attribute.NamedArguments.Length).ToList();
                for (int i = 1; i < allMappers.Count; i++)
                {
                    var att = allMappers[i];
                    reporter.Invoke(Diagnostic.Create(DiagnosticDescriptors.DuplicateMapper, att.SourceLocation, att.MapperInfo.Source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), att.MapperInfo.Target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }

        }
        private static void AnalyzeAttributeAndMethods(Action<Diagnostic> reporter, ImmutableArray<AttributeDefineInfo> attributes, ImmutableArray<MethodInvokeInfo> methods)
        {
            foreach (var att in attributes)
            {
                if (!methods.Any(p => p.MapperInfo.TheSameType(att.MapperInfo)))
                {
                    reporter.Invoke(Diagnostic.Create(DiagnosticDescriptors.MapperNotUsed, att.SourceLocation, att.MapperInfo.Source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), att.MapperInfo.Target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }
            foreach (var method in methods)
            {
                if (!attributes.Any(p => p.MapperInfo.TheSameType(method.MapperInfo)))
                {
                    reporter.Invoke(Diagnostic.Create(DiagnosticDescriptors.MissingMapper, method.SourceLocation, method.MapperInfo.Source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), method.MapperInfo.Target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }
        }
        internal static MapperInfo GetMethodMapperInfo(IMethodSymbol methodSymbol)
        {

            var receiveType = methodSymbol.ReceiverType as INamedTypeSymbol;
            var returnType = methodSymbol.ReturnType as INamedTypeSymbol;
            if (methodSymbol.ReturnsVoid)
            {
                //copy method
                returnType = methodSymbol.Parameters.First().Type as INamedTypeSymbol;
                if (receiveType.IsGenericType && returnType.IsGenericType)
                {
                    var unboundType = receiveType.ConstructUnboundGenericType();
                    var returnUnboundType = returnType.ConstructUnboundGenericType();
                    if (unboundType.SafeEquals(typeof(IDictionary<,>)) && returnUnboundType.SafeEquals(typeof(IDictionary<,>)))
                    {
                        return CreateMapperInfo(GetSecondArgument(receiveType), GetSecondArgument(returnType), Kind.Update);
                    }
                    if (unboundType.SafeEquals(typeof(IEnumerable<>)) && returnUnboundType.SafeEquals(typeof(ICollection<>)))
                    {
                        return CreateMapperInfo(GetFirstArgument(receiveType), GetFirstArgument(returnType), Kind.Update);
                    }

                }
                return CreateMapperInfo(receiveType, returnType, Kind.Update);
            }

            if (receiveType.IsGenericType && returnType.IsGenericType)
            {
                var unboundType = receiveType.ConstructUnboundGenericType();
                var returnUnboundType = returnType.ConstructUnboundGenericType();
                // convert enumerable 
                if (unboundType.SafeEquals(typeof(IEnumerable<>)) && returnUnboundType.SafeEquals(typeof(IEnumerable<>)))
                {
                    return CreateMapperInfo(GetFirstArgument(receiveType), GetFirstArgument(returnType), Kind.Convert);

                }
                if (unboundType.SafeEquals(typeof(IQueryable<>)) && returnUnboundType.SafeEquals(typeof(IQueryable<>)))
                {
                    return CreateMapperInfo(GetFirstArgument(receiveType), GetFirstArgument(returnType), Kind.Query);
                }
            }
            return CreateMapperInfo(receiveType, returnType, Kind.Query);
            INamedTypeSymbol GetFirstArgument(INamedTypeSymbol symbol)
            {
                return symbol.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
            }
            INamedTypeSymbol GetSecondArgument(INamedTypeSymbol symbol)
            {
                return symbol.TypeArguments.Skip(1).FirstOrDefault() as INamedTypeSymbol;
            }
            MapperInfo CreateMapperInfo(INamedTypeSymbol source, INamedTypeSymbol target, Kind kind)
            {
                if (source != null && target != null)
                {
                    return new MapperInfo { Source = source, Target = target, Kind = kind };
                }
                return null;
            }
        }

    }
    internal class MethodInvokeInfo
    {
        public MapperInfo MapperInfo { get; set; }
        public Location SourceLocation { get; set; }
        public IMethodSymbol Method { get; set; }
    }
    internal class AttributeDefineInfo
    {
        public MapperInfo MapperInfo { get; set; }
        public Location SourceLocation { get; set; }
        public AttributeData Attribute { get; set; }
    }
    internal class MapperInfo : IEquatable<MapperInfo>
    {
        public INamedTypeSymbol Source { get; set; }
        public INamedTypeSymbol Target { get; set; }
        public Kind Kind { get; set; }

        public bool Equals(MapperInfo other)
        {
            return this.Source.SafeEquals(other.Source) && this.Target.SafeEquals(other.Target) && this.Kind == other.Kind;
        }
        public bool Contains(MapperInfo other)
        {
            return this.Source.SafeEquals(other.Source) && this.Target.SafeEquals(other.Target) && (this.Kind & other.Kind) == other.Kind;
        }
        public bool TheSameType(MapperInfo other)
        {
            return this.Source.SafeEquals(other.Source) && this.Target.SafeEquals(other.Target);

        }
        public string GetMapperKey()
        {
            return $"{this.Source.ToDisplayFullString()}-{this.Target.ToDisplayFullString()}";
        }
    }
    [Flags]
    internal enum Kind
    {
        Query = 1,
        Convert = 2,
        Update = 4,
    }

}
