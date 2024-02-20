using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlyTiger.Mapper.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper.CodeFix
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MapperCodeFix)), Shared]
    public class MapperCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(MapperAnalyzer.DiagnosticId);
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics.Where(p => p.Id == MapperAnalyzer.DiagnosticId))
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: "Remove Mapper",
                        createChangedDocument: c => RemoveAttributeAsync(context.Document, diagnostic, c),
                        equivalenceKey: "MapperCodeFix_RemoveMapper"),
                    diagnostic);
            }
            return Task.CompletedTask;


        }
        
        private static async Task<Document> RemoveAttributeAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var attribute = root.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<AttributeSyntax>();

            var parent = attribute.Parent;

            if(parent is AttributeListSyntax attributeList)
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

           // var newRoot = root.RemoveNode(attribute, SyntaxRemoveOptions.KeepNoTrivia);

            return document;


            //var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            //var attribute = root.FindNode(diagnostic.Location.SourceSpan).FirstAncestorOrSelf<AttributeSyntax>();

            //// 查找Attribute定义外层所在的语法节点，例如 ClassDeclarationSyntax
            //var containerNode = attribute.Ancestors().FirstOrDefault(node => node is ClassDeclarationSyntax || node is MethodDeclarationSyntax || node is PropertyDeclarationSyntax);

            //var newRoot = root.RemoveNode(attribute, SyntaxRemoveOptions.KeepNoTrivia);

            //// 删除Attribute定义外层的中括号
            //newRoot = newRoot.RemoveNode(containerNode, SyntaxRemoveOptions.KeepNoTrivia);

            //return document.WithSyntaxRoot(newRoot);
        }
    }
}
