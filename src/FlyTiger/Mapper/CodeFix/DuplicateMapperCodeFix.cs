using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper.CodeFix
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingMapperCodeFix)), Shared]
    internal class DuplicateMapperCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticDescriptors.DuplicateMapper.Id);

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics.Where(p => p.Id == DiagnosticDescriptors.DuplicateMapper.Id))
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Resources.RemoveMapper,
                        createChangedDocument: c => RemoveAttributeAsync(context.Document, diagnostic, c),
                        equivalenceKey: $"FlyTiger.Mapper.{nameof(DuplicateMapperCodeFix)}"),
                    diagnostic);
            }
            return Task.CompletedTask;
        }

        private static async Task<Document> RemoveAttributeAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var attribute = root.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<AttributeSyntax>();
            var parent = attribute.Parent;
            if (parent is AttributeListSyntax attributeList)
            {
                if (attributeList.Attributes.Count <= 1)
                {
                    var newRoot = root.RemoveNode(attributeList, SyntaxRemoveOptions.KeepNoTrivia);
                    return document.WithSyntaxRoot(newRoot);
                }
                else
                {
                    var newRoot = root.RemoveNode(attribute, SyntaxRemoveOptions.KeepNoTrivia);
                    return document.WithSyntaxRoot(newRoot);
                }
            }
            return document;
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }
    }
}
