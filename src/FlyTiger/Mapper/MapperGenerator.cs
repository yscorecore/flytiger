using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FlyTiger.Mapper.Generators;
using Microsoft.CodeAnalysis;
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

            // Use ForAttributeWithMetadataName for reliable attribute matching in VS design-time builds
            var mapperAttributes = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    AttributeFullName,
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: (ctx, _) => ctx.Attributes)
                .SelectMany((attrs, _) => attrs)
                .Collect();

            // Combine with compilation so we can inspect referenced assemblies
            var compilationAndAttributes = context.CompilationProvider.Combine(mapperAttributes);


            context.RegisterSourceOutput(compilationAndAttributes, (spc, source) =>
            {
                var (compilation, attributes) = source;
                if (attributes.IsDefaultOrEmpty)
                {
                    return;
                }
                var codeWriter = new CodeWriter(compilation, spc);
                var attributeList = attributes.ToList();
                if (attributeList.Count > 0)
                {
                    var codeFile = new CodeFile
                    {
                        BasicName = MapperExtensionName,
                        Content = BuildMapperContent(codeWriter, attributeList),
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

    }
}
