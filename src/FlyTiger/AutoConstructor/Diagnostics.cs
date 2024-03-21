using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.AutoConstructor
{
    internal static class Diagnostics
    {
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
                DiagnosticDescriptors.InitializeMethodShouldNotBeStatic,
            FindMethodInitializeAttributeLocation(method), method.Name));
        }
        public static void InitializeMethodShouldReturnVoid(this GeneratorExecutionContext context, IMethodSymbol method)
        {
            context.ReportDiagnostic(Diagnostic.Create(
               DiagnosticDescriptors.InitializeMethodShouldReturnVoid,
             FindMethodInitializeAttributeLocation(method), method.Name));
        }
        public static void InitializeMethodShouldOnlyOne(this GeneratorExecutionContext context, ITypeSymbol clazz, IEnumerable<IMethodSymbol> methods)
        {
            foreach (var method in methods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                  DiagnosticDescriptors.InitializeMethodShouldOnlyOne,
                FindMethodInitializeAttributeLocation(method), methods.Count(), clazz.Name));
            }

        }
        public static void InitializeMethodShouldHasNoneArguments(this GeneratorExecutionContext context, IMethodSymbol method)
        {
            context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.InitializeMethodShouldHasNoneArguments,
            FindMethodInitializeAttributeLocation(method), method.Name));
        }
    }
}
