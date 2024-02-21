using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FlyTiger.Mapper.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper.CodeFix
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingMapperCodeFix)), Shared]
    internal class MissingMapperCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(DiagnosticDescriptors.MissingMapper.Id);
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics.Where(p => p.Id == DiagnosticDescriptors.MissingMapper.Id))
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Resources.AddMapper,
                        createChangedDocument: c => AddMapperAsync(context.Document, diagnostic, c),
                        equivalenceKey: $"FlyTiger.Mapper.{nameof(MissingMapperCodeFix)}"),
                    diagnostic);
            }
            return Task.CompletedTask;
        }
        private static async Task<Document> AddMapperAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var methodInvocation = root.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<InvocationExpressionSyntax>();
            var classDeclarationSyntax = methodInvocation.Ancestors().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            var semanticModel = await document.GetSemanticModelAsync();
            if (classDeclarationSyntax != null && semanticModel != null)
            {
                if (semanticModel.GetSymbolInfo(methodInvocation).Symbol is IMethodSymbol methodSymbol)
                {
                    var mapperInfo = MapperAnalyzer.GetMethodMapperInfo(methodSymbol);
                    if (mapperInfo != null)
                    {
                        var arguments = SyntaxFactory.ParseAttributeArgumentList($"(typeof({mapperInfo.Source.Name}), typeof({mapperInfo.Target.Name}))");
                        var attributeList = SyntaxFactory.AttributeList(
                           SyntaxFactory.SingletonSeparatedList(
                               SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(MapperGenerator.AttributeShortName), arguments
                           )
                        ));
                        var newClassDeclaration = classDeclarationSyntax
                            .AddAttributeLists(attributeList);
                        var newRoot = root.ReplaceNode(classDeclarationSyntax, newClassDeclaration);
                        var newDocument = document.WithSyntaxRoot(newRoot);
                        return newDocument;
                    }
                }
            }
            return document;
        }
    }
}
