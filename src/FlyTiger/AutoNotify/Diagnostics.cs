using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.AutoNotify
{
    internal static class Diagnostics
    {
        const string Category = "FlyTiger.AutoNofify";

        static Location FindFieldAutoNofityAttributeLocation(IFieldSymbol field)
        {
            var attribute = field.GetAttributes()
                .Where(p => p.AttributeClass.Is(AutoNotifyGenerator.AttributeFullName))
                .FirstOrDefault();

            if (attribute != null)
            {
                var syntaxReference = attribute.ApplicationSyntaxReference;
                var syntaxNode = syntaxReference.GetSyntax();
                var location = syntaxNode.GetLocation();
                return location;
            }

            return Location.None;
        }

        public static void InvalidPropertyName(this GeneratorExecutionContext context, IFieldSymbol field, string propName)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT1001",
                "Invalid property name",
                "Invalid property name '{0}'.",
                Category,
                DiagnosticSeverity.Error,
                true),
                FindFieldAutoNofityAttributeLocation(field), propName));
        }
        public static void PropertyNameAlreadyExists(this GeneratorExecutionContext context, INamedTypeSymbol clazz, IFieldSymbol field, string propName)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT1002",
                "The property name already exists",
                "The property name '{0}' already exists in the class '{1}'.",
                Category,
                DiagnosticSeverity.Error,
                true),
                FindFieldAutoNofityAttributeLocation(field), propName, clazz.Name));
        }
    }
}
