using System.Collections.Generic;
using System.Linq;
using FlyTiger.Mapper.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper
{
    [Generator]
    partial class MapperGenerator : IIncrementalGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        public const string MapperExtensionName = "MapperExtensions";



        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Post init - add attribute definitions
            context.RegisterPostInitializationOutput((i) =>
            {
                i.AddSource($"{AttributeFullName}.g.cs", AttributeCode);
                i.AddSource($"{EFCoreQueryableExtensionsFullName}.g.cs", EFCoreExtensionsCode);
            });

            // Syntax provider: find candidate class declarations
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds && cds.AttributeLists.Any(),
                    transform: (genCtx, ct) =>
                    {
                        var classDecl = (ClassDeclarationSyntax)genCtx.Node;
                        // 在 transform 阶段直接检查是否有 [Mapper] 属性（支持多种写法）
                        if (classDecl.AttributeLists.SelectMany(al => al.Attributes)
                            .Any(a =>
                                a.Name.ToString() == AttributeName ||
                                a.Name.ToString() == AttributeShortName ||
                                a.Name.ToString() == $"{NameSpaceName}.{AttributeName}" ||
                                a.Name.ToString() == $"{NameSpaceName}.{AttributeShortName}"))
                        {
                            return classDecl;
                        }
                        return null;
                    })
                .Where(s => s != null)
                .Collect();

            // Combine with compilation so we can inspect referenced assemblies
            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations);


            context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
            {
                var (compilation, classes) = source;
                if (classes.IsDefaultOrEmpty)
                {
                    return;
                }
                var codeWriter = new CodeWriter(compilation, spc);
                var attributes = FindAllAttributes(codeWriter, classes);
                if (attributes.Count > 0)
                {
                    var codeFile = new CodeFile
                    {
                        BasicName = MapperExtensionName,
                        Content = BuildMapperContent(codeWriter, attributes),
                    };
                    codeWriter.WriteCodeFile(codeFile);
                }
            });
        }
        private string BuildMapperContent(CodeWriter codeWriter, IList<AttributeData> attributeDatas)
        {
            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder("CS1591");
            // using
            codeBuilder.AppendCodeLines("using System;");
            codeBuilder.AppendCodeLines("using System.Collections.Immutable;");
            codeBuilder.AppendCodeLines("using System.Collections.Generic;");
            codeBuilder.AppendCodeLines("using System.Linq;");
            codeBuilder.AppendCodeLines("using System.Linq.Expressions;");
            // namespace
            codeBuilder.AppendCodeLines($"namespace {NameSpaceName}");
            codeBuilder.BeginSegment();
            // class
            codeBuilder.AppendCodeLines($@"static class {MapperExtensionName}");
            codeBuilder.BeginSegment();
            // common
            new CommonFunctionGenerator().AppendFunctions(codeBuilder);

            var rootMappingInfos = attributeDatas.Select(ConvertMappingInfo.FromAttributeData).ToList();
            // convert
            new ConvertFunctionGenerator().AppendFunctions(codeWriter, codeBuilder, rootMappingInfos, attributeDatas);
            // generic
            new GenericFunctionGenerator().AppendFunctions(codeWriter, codeBuilder, rootMappingInfos);
            codeBuilder.EndAllSegments();
            return codeBuilder.ToString();
        }
        private IList<AttributeData> FindAllAttributes(CodeWriter codeWriter, IList<ClassDeclarationSyntax> classDeclarations)
        {
            var attributes = new List<AttributeData>();
            foreach (var classDecl in classDeclarations)
            {
                var model = codeWriter.Compilation.GetSemanticModel(classDecl.SyntaxTree);
                if (model.GetDeclaredSymbol(classDecl) is INamedTypeSymbol classSymbol)
                {
                    attributes.AddRange(classSymbol.GetAttributes().Where(t => t.AttributeClass.Is(AttributeFullName)));
                }
            }
            return attributes;
        }

    }
}
