using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.AutoConstructor
{
    internal static class Diagnostics
    {
        const string Category = "FlyTiger.AutoCtor";
        static Location FindMethodInitializeAttributeLocation(IMethodSymbol method)
        {
            var attribute = method.GetAttributes()
                .Where(p => p.AttributeClass.Is(AutoConstructorGenerator.InitializeAttributeFullName))
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
        public static void InitializeMethodShouldNotBeStatic(this GeneratorExecutionContext context, IMethodSymbol method)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("FT0001",
                    "The initialization method should be an instance method",
                    "The method '{0}' has been marked as initialization method, but it is a static method, instance method is required in here.",
                    Category,
                    DiagnosticSeverity.Warning,
                    true),
            FindMethodInitializeAttributeLocation(method), method.Name));
        }
        public static void InitializeMethodShouldReturnVoid(this GeneratorExecutionContext context, IMethodSymbol method)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("FT0002",
                    "The initialization method should return void",
                    "The method '{0}' has been marked as initialization method, but the return type is not void.",
                    Category,
                    DiagnosticSeverity.Warning,
                    true),
             FindMethodInitializeAttributeLocation(method), method.Name));
        }
        public static void InitializeMethodShouldOnlyOne(this GeneratorExecutionContext context, ITypeSymbol clazz, IEnumerable<IMethodSymbol> methods)
        {
            foreach (var method in methods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                  new DiagnosticDescriptor("FT0003",
                      "Multiple initialization methods exist in one class",
                      "There can be at most one initialization method, but {0} initialization methods has been found in class '{1}'.",
                      Category,
                      DiagnosticSeverity.Warning,
                      true),
                FindMethodInitializeAttributeLocation(method), methods.Count(), clazz.Name));
            }

        }
        public static void InitializeMethodShouldHasNoneArguments(this GeneratorExecutionContext context, IMethodSymbol method)
        {
            context.ReportDiagnostic(Diagnostic.Create(
             new DiagnosticDescriptor("FT0004",
                 "The initialization method should have none parameters",
                 "The method '{0}' has been marked as initialization method, but has parameters, none parameters is required in here.",
                 Category,
                 DiagnosticSeverity.Error,
                 true),
            FindMethodInitializeAttributeLocation(method), method.Name));
        }
    }
}
