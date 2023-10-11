using System.Collections.Generic;
using System.Linq;
using FlyTiger.Mapper.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper
{
    [Generator]
    partial class MapperGenerator : ISourceGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
    
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization((i) =>
            {
                i.AddSource($"{AttributeFullName}.g.cs", AttributeCode);
                i.AddSource($"{EFCoreQueryableExtensionsFullName}.g.cs", EFCoreExtensionsCode);
            });
            context.RegisterForSyntaxNotifications(() => new MapperSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is MapperSyntaxReceiver receiver))
                return;
            var codeWriter = new CodeWriter(context);

            var attributes = FindAllAttributes(codeWriter, receiver.CandidateClasses);
            if (attributes.Count > 0)
            {
                var codeFile = new CodeFile
                {
                    BasicName = "MapperExtensions",
                    Content = BuildMapperContent(codeWriter, attributes),
                };
                codeWriter.WriteCodeFile(codeFile);
            }
        }

        private string BuildMapperContent(CodeWriter codeWriter, IList<AttributeData> attributeDatas)
        {
            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder();
            // using
            codeBuilder.AppendCodeLines("using System;");
            codeBuilder.AppendCodeLines("using System.Collections.Generic;");
            codeBuilder.AppendCodeLines("using System.Linq;");
            codeBuilder.AppendCodeLines("using System.Linq.Expressions;");
            // namespace
            codeBuilder.AppendCodeLines($"namespace {NameSpaceName}");
            codeBuilder.BeginSegment();
            // class
            codeBuilder.AppendCodeLines($@"static class MapperExtensions");
            codeBuilder.BeginSegment();
            // common
            new CommonFunctionGenerator().AppendFunctions(codeBuilder);
            // convert
            new ConvertFunctionGenerator().AppendFunctions(codeWriter, codeBuilder, attributeDatas);
            // generic
            new GenericFunctionGenerator().AppendFunctions(codeWriter, codeBuilder, attributeDatas);
            codeBuilder.EndAllSegments();
            return codeBuilder.ToString();
        }
        private IList<AttributeData> FindAllAttributes(CodeWriter codeWriter, IList<ClassDeclarationSyntax> candidateClasses)
        {
            return codeWriter.GetAllClassSymbolsIgnoreRepeated(candidateClasses)
                  .SelectMany(p => p.GetAttributes().Where(t => t.AttributeClass.Is(AttributeFullName)))
                  .ToList();
        }


        private class MapperSyntaxReceiver : ISyntaxReceiver
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
}
