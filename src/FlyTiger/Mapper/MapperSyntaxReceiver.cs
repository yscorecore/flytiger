using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger
{
    internal class MapperSyntaxReceiver : ISyntaxReceiver
    {
        public IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                classDeclarationSyntax.AttributeLists.Any())
            {
                CandidateClasses.Add(classDeclarationSyntax);
            }
        }
    }

}
