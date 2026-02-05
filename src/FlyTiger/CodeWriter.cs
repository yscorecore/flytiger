using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FlyTiger
{
    class CodeWriter
    {
        public CodeWriter(GeneratorExecutionContext context)
        {
            this.AddSource = context.AddSource;
            this.Compilation = context.Compilation;
            this.ReportDiagnostic = context.ReportDiagnostic;
            this.ParseOptions = context.ParseOptions;
        }
        public CodeWriter(ParseOptions parseOptions, Compilation compilation, SourceProductionContext context)
        {
            this.Compilation = compilation;
            this.AddSource = context.AddSource;
            this.ReportDiagnostic = context.ReportDiagnostic;
            this.ParseOptions = parseOptions;
        }
        public string CodeFileSuffix { get; set; } = "g.cs";

        public string CodeFilePrefix { get; set; } = nameof(FlyTiger);
        public Compilation Compilation { get; private set; }
        public ParseOptions ParseOptions { get; }

        public Action<string,string> AddSource { get; private set; }
        public Action<Diagnostic> ReportDiagnostic { get; private set; }

        private Dictionary<string, int> fileNames = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

        public void WriteCodeFile(CodeFile codeFile)
        {
            if (codeFile == null) return;

            fileNames.TryGetValue(codeFile.BasicName, out var i);
            var name = i == 0 ? codeFile.BasicName : $"{codeFile.BasicName}.{i + 1}";
            fileNames[codeFile.BasicName] = i + 1;

            this.AddSource($"{CodeFilePrefix}.{name}.{CodeFileSuffix}", codeFile.Content);

            this.Compilation = this.Compilation.AddSyntaxTrees(
               CSharpSyntaxTree.ParseText(SourceText.From(codeFile.Content, Encoding.UTF8), (CSharpParseOptions)this.ParseOptions));
        }
    }
    static class CodeWriterExtensions
    {
        class ClassSyntaxCachedInfo
        {
            public ClassDeclarationSyntax Syntax { get; set; }

            public INamedTypeSymbol NameTypedSymbol { get; set; }

            public string QualifiedName { get; set; }

            public bool Handled { get; set; }
        }
        public static void ForeachClassSyntax(this CodeWriter codeWriter, IEnumerable<ClassDeclarationSyntax> classSyntax, Func<INamedTypeSymbol, CodeWriter, CodeFile> codeFileFactory)
        {
            _ = codeFileFactory ?? throw new ArgumentNullException(nameof(codeFileFactory));
            var dic = new Dictionary<string, ClassSyntaxCachedInfo>();

            foreach (var clazz in classSyntax ?? Enumerable.Empty<ClassDeclarationSyntax>())
            {
                SemanticModel model = codeWriter.Compilation.GetSemanticModel(clazz.SyntaxTree);
                var clazzSymbol = model.GetDeclaredSymbol(clazz);

                var digs = model.GetDeclarationDiagnostics();
                if (digs.Length > 0)
                {
                    // Log warning..
                }

                var qualifiedName = clazzSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                dic[qualifiedName] = new ClassSyntaxCachedInfo
                {
                    Handled = false,
                    NameTypedSymbol = clazzSymbol,
                    Syntax = clazz,
                    QualifiedName = qualifiedName,
                };
            }

            foreach (var classCachedInfo in dic.Values)
            {
                Visit(classCachedInfo);
            }
            void Visit(ClassSyntaxCachedInfo value)
            {
                if (value.Handled) return;
                if (value.NameTypedSymbol.BaseType != null)
                {
                    var baseQualifiedName = value.NameTypedSymbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    if (dic.TryGetValue(baseQualifiedName, out var baseCachedInfo))
                    {
                        Visit(baseCachedInfo);
                    }
                }
                SemanticModel model = codeWriter.Compilation.GetSemanticModel(value.Syntax.SyntaxTree);
                var clazzSymbol = model.GetDeclaredSymbol(value.Syntax);
                codeWriter.WriteCodeFile(codeFileFactory(clazzSymbol, codeWriter));
                value.Handled = true;
            }
        }
        class ClassSyntaxCachedInfo2
        {
            public INamedTypeSymbol NameTypedSymbol { get; set; }

            //public string QualifiedName { get; set; }

            public bool Handled { get; set; }
        }
  
        public static void ForeachClassByInheritanceOrder(this CodeWriter codeWriter, IEnumerable<INamedTypeSymbol> classSyntax, Func<INamedTypeSymbol, CodeWriter, CodeFile> codeFileFactory)
        {
            _ = codeFileFactory ?? throw new ArgumentNullException(nameof(codeFileFactory));
            var dic = new Dictionary<INamedTypeSymbol, ClassSyntaxCachedInfo2>(SymbolEqualityComparer.Default);

            foreach (var clazz in classSyntax ?? Enumerable.Empty<INamedTypeSymbol>())
            {
                dic[clazz] = new ClassSyntaxCachedInfo2
                { 
                    NameTypedSymbol = clazz
                };
            }

            foreach (var classCachedInfo in dic.Values)
            {
                Visit(classCachedInfo);
            }
            void Visit(ClassSyntaxCachedInfo2 value)
            {
                if (value.Handled) return;
                if (value.NameTypedSymbol.BaseType != null)
                {
                    if (dic.TryGetValue(value.NameTypedSymbol.BaseType, out var baseCachedInfo))
                    {
                        Visit(baseCachedInfo);
                    }
                }
                ;
                codeWriter.WriteCodeFile(codeFileFactory(value.NameTypedSymbol, codeWriter));
                value.Handled = true;
            }
        }
    }
}
