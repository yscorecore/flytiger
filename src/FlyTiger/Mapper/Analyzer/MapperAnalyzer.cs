using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FlyTiger.Mapper.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FlyTiger.Mapper.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MapperAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "FT50000";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;
            var allAttributes = GetAllMapperAttribute(context, classDeclaration).ToImmutableArray();
            var allMethods = GetAllMapperMethodInvoke(context, classDeclaration).ToImmutableArray();
            AnalyzeAttribute(context, allAttributes, allMethods);
        }



        private IEnumerable<AttributeData> GetAllMapperAttribute(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            var classDefine = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            return classDefine.GetAttributes().Where(p => p.AttributeClass.Is(MapperGenerator.AttributeFullName));

        }
        private IEnumerable<IMethodSymbol> GetAllMapperMethodInvoke(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            var classDefine = context.SemanticModel.GetSymbolInfo(classDeclaration);

            foreach (var member in classDeclaration.Members)
            {
                foreach (var methodInvocation in member.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (context.SemanticModel.GetSymbolInfo(methodInvocation).Symbol is IMethodSymbol methodSymbol &&
                        methodSymbol.Name == GenericFunctionGenerator.ConvertGenericMethodName &&
                        methodSymbol.ContainingType.Name == MapperGenerator.MapperExtensionName &&
                        methodSymbol.ReceiverType != null)
                    {
                        yield return methodSymbol;
                    }
                }
            }
        }

        private void AnalyzeAttribute(SyntaxNodeAnalysisContext context, ImmutableArray<AttributeData> attributes, ImmutableArray<IMethodSymbol> methods)
        {
            var attributeMaps = attributes.Select(p => new
            {
                Attr = p,
                MapperInfo = CreateMapperInfo(p)
            }).ToList();
            var methodMaps = methods.Select(p => new
            {
                Method = p,
                MapperInfo = GetMethodMapperInfo(p)
            }).ToList();

            foreach (var att in attributeMaps)
            {
                if (!methodMaps.Any(p => p.MapperInfo.TheSameType(att.MapperInfo)))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, FindAttributeLocation(att.Attr), att.MapperInfo.Source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), att.MapperInfo.Target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }
            foreach (var method in methodMaps)
            {
                if (!attributeMaps.Any(p => p.MapperInfo.TheSameType(method.MapperInfo)))
                {
                   // context.ReportDiagnostic(Diagnostic.Create(Rule, FindAttributeLocation(att.Attr), att.MapperInfo.Source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), att.MapperInfo.Target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
                }
            }
        }
        private MapperInfo GetMethodMapperInfo(IMethodSymbol methodSymbol)
        {
            var receiveType = methodSymbol.ReceiverType as INamedTypeSymbol;
            var returnType = methodSymbol.ReturnType as INamedTypeSymbol;
            if (returnType is null)
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
        }

        private MapperInfo CreateMapperInfo(INamedTypeSymbol source, INamedTypeSymbol target, Kind kind)
        {
            if (source != null && target != null)
            {
                return new MapperInfo { Source = source, Target = target, Kind = kind };
            }
            return null;
        }
        private MapperInfo CreateMapperInfo(AttributeData att)
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


        static Location FindAttributeLocation(AttributeData attributeData)
        {
            var syntaxReference = attributeData.ApplicationSyntaxReference;
            var syntaxNode = syntaxReference.GetSyntax();
            return syntaxNode.GetLocation();
        }
        static INamedTypeSymbol GetFirstArgument(INamedTypeSymbol symbol)
        {
            return symbol.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
        }
        static INamedTypeSymbol GetSecondArgument(INamedTypeSymbol symbol)
        {
            return symbol.TypeArguments.Skip(1).FirstOrDefault() as INamedTypeSymbol;
        }

    }
    class MapperInfo : IEquatable<MapperInfo>
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
    }
    [Flags]
    enum Kind
    {
        Query = 1,
        Convert = 2,
        Update = 4,
    }
}
