using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{
    internal static class Diagnostics
    {
        const string Category = "FlyTiger.Mapper";
        static Location FindMemberLocation(ISymbol fieldOrProperty)
        {
            return fieldOrProperty.Locations.FirstOrDefault() ?? Location.None;
        }
        static string GetTypeName(this INamedTypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);
        public static void ReportCanNotMapper(this GeneratorExecutionContext context, AttributeData attributeData)
        {
            //context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
            //    "FT4001",
            //    "Can not create mapper",
            //    "Target property '{0}' in class '{1}' is not filled, source class is '{2}'.",
            //    Category,
            //     DiagnosticSeverity.Error,
            //    true),
            //    FindMemberLocation(targetProperty)));
        }

        public static void ReportTargetPropertyNotFilled(this GeneratorExecutionContext context, ISymbol targetProperty, INamedTypeSymbol source, INamedTypeSymbol target, DiagnosticSeverity diagnosticSeverity)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT4002",
                "Target property is not filled",
                "Target property '{0}' in class '{1}' is not filled, source class is '{2}'.",
                Category,
                diagnosticSeverity,
                true),
                FindMemberLocation(targetProperty), targetProperty.Name, target.GetTypeName(), source.GetTypeName()));
        }
        public static void ReportSourcePropertyNotMapped(this GeneratorExecutionContext context, ISymbol sourceProperty, INamedTypeSymbol source, INamedTypeSymbol target, DiagnosticSeverity diagnosticSeverity)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT4003",
                "Source property is not mapped",
                "Source property '{0}' in class '{1}' is not mapped, target class is '{2}'.",
                Category,
                diagnosticSeverity,
                true),
                FindMemberLocation(sourceProperty), sourceProperty.Name, source.GetTypeName(), target.GetTypeName()));
        }

    }
}
