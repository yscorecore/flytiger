using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger.SingletonPattern
{
    internal static class Diagnostics
    {
        const string Category = "FlyTiger.SingletonPattern";
        static Location FindSingletonPatternAttributeLocation(INamedTypeSymbol clazz)
        {
            var attribute = clazz.GetAttributes()
                .Where(p => p.AttributeClass.Is(SingletonPatternGenerator.AttributeFullName))
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
        public static void AlreadyExistsEmptyConstructor(this CodeWriter context, INamedTypeSymbol classType)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
               "FT2001",
               "The class already exists empty constructor",
               "The class '{0}' already exists empty constructor, can not generate duplicate empty constructor.",
               Category,
               DiagnosticSeverity.Error,
            true),
               FindSingletonPatternAttributeLocation(classType), classType.Name));
        }
        public static void AlreadyExistsEmptyConstructor(this SourceProductionContext context, INamedTypeSymbol classType)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
               "FT2001",
               "The class already exists empty constructor",
               "The class '{0}' already exists empty constructor, can not generate duplicate empty constructor.",
               Category,
               DiagnosticSeverity.Error,
            true),
               FindSingletonPatternAttributeLocation(classType), classType.Name));
        }

        public static void InvalidInstancePropertyName(this CodeWriter context, INamedTypeSymbol classType, string propName)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT2001",
                "Invalid instance property name",
                "Invalid instance property name '{0}'.",
                Category,
                DiagnosticSeverity.Error,
                true),
                FindSingletonPatternAttributeLocation(classType), propName));
        }
        public static void InvalidInstancePropertyName(this SourceProductionContext context, INamedTypeSymbol classType, string propName)
        {
            context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                "FT2001",
                "Invalid instance property name",
                "Invalid instance property name '{0}'.",
                Category,
                DiagnosticSeverity.Error,
                true),
                FindSingletonPatternAttributeLocation(classType), propName));
        }
    }
}
