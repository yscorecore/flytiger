using System;
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
        static Location FindAttributeLocation(AttributeData attributeData)
        {
            var syntaxReference = attributeData.ApplicationSyntaxReference;
            var syntaxNode = syntaxReference.GetSyntax();
            return syntaxNode.GetLocation();
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

        public static void ReportTargetPropertyNotFilled(this GeneratorExecutionContext context, IPropertySymbol targetProperty, ITypeSymbol source, ITypeSymbol target, DiagnosticSeverity diagnosticSeverity = DiagnosticSeverity.Error)
        {

            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT4002",
                "Target property is not filled",
                "Target property '{0}' in class '{1}' is not filled, source class is '{2}'.",
                Category,
                diagnosticSeverity,
                true),
                FindMemberLocation(targetProperty), targetProperty.Name, target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
        public static void ReportTargetPropertyNotFilledBecauseIsGetOnly(this GeneratorExecutionContext context, IPropertySymbol targetProperty, ITypeSymbol source, ITypeSymbol target, DiagnosticSeverity diagnosticSeverity = DiagnosticSeverity.Error)
        {
            //context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
            //    "FT4004",
            //    "Target readonly property can not been filled",
            //    "Target readonly property '{0}' in class can not be filled, source class is '{2}'.",
            //    Category,
            //    diagnosticSeverity,
            //    true),
            //    FindMemberLocation(targetProperty), targetProperty.Name, target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
        public static void ReportSourcePropertyNotMapped(this GeneratorExecutionContext context, IPropertySymbol sourceProperty, ITypeSymbol source, ITypeSymbol target, DiagnosticSeverity diagnosticSeverity = DiagnosticSeverity.Error)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT4003",
                "Source property is not mapped",
                "Source property '{0}' in class '{1}' is not mapped, target class is '{2}'.",
                Category,
                diagnosticSeverity,
                true),
                FindMemberLocation(sourceProperty), sourceProperty.Name, source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
        public static void ReportTargetStructCanNotCopy(this GeneratorExecutionContext context, AttributeData attribute, ITypeSymbol source, ITypeSymbol target, DiagnosticSeverity diagnosticSeverity = DiagnosticSeverity.Error)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT4005",
                "Can not copy to struct type",
                "Target type is struct, can not copy '{0}' to '{1}'.",
                Category,
                diagnosticSeverity,
                true),
                FindAttributeLocation(attribute), source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat), target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
    }
}
