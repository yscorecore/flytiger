using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger
{
    static class KnifeDiagnostic
    {

        public static class Singleton
        {
            public static Diagnostic AlreadyExistsConstructor(INamedTypeSymbol classType)
            {
                DiagnosticDescriptor desc = new DiagnosticDescriptor("KF0001", SingletonPatternGenerator.AttributeFullName, $"The type '{classType.ToDisplayString()}' has already exists constructor, can not use '{SingletonPatternGenerator.AttributeFullName}'.", "Error", DiagnosticSeverity.Error, true);
                return Diagnostic.Create(desc, null);
            }
        }
        public static class AutoNotify
        {
            public static Diagnostic InvalidPropertyName(string propName)
            {
                DiagnosticDescriptor desc = new DiagnosticDescriptor("KF1001", AutoNotifyGenerator.AttributeFullName, $"Invalid property name '{propName}'.", "Error", DiagnosticSeverity.Error, true);
                return Diagnostic.Create(desc, null);
            }
            public static Diagnostic PropertyNameEqualFieldName(string propName)
            {
                DiagnosticDescriptor desc = new DiagnosticDescriptor("KF1002", AutoNotifyGenerator.AttributeFullName, $"The property name '{propName}' and field name are equal.", "Error", DiagnosticSeverity.Error, true);
                return Diagnostic.Create(desc, null);
            }
        }
    }
}
