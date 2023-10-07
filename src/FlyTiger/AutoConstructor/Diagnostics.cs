using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger.AutoConstructor
{
    internal static class Diagnostics
    {
        public static void InitializeMethodShouldNotBeStatic(this GeneratorExecutionContext context, Location location)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("FT00001", "Error", "The initialize method should be instance method.", "Category", DiagnosticSeverity.Warning, true),
            location));
        }
        public static void InitializeMethodShouldReturnVoid(this GeneratorExecutionContext context, Location location)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("FT00002", "Error", "The initialize method should return void.", "Category", DiagnosticSeverity.Warning, true),
            location));
        }
        public static void InitializeMethodShouldHasNoneArguments(this GeneratorExecutionContext context, Location location)
        {
            context.ReportDiagnostic(Diagnostic.Create(
             new DiagnosticDescriptor("FT00003", "Error", "The initialize method should have none parameter.", "Category", DiagnosticSeverity.Warning, true),
         location));
        }
    }
}
