using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.SingletonPattern
{
    [Generator]
    class SingletonPatternGenerator : IIncrementalGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        const string AttributeName = "SingletonPatternAttribute";
        const string PropertyName = "InstancePropertyName";
        internal static string AttributeFullName = $"{NameSpaceName}.{AttributeName}";
        const string AttributeCode = @"using System;
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class SingletonPatternAttribute : Attribute
    {
        public string InstancePropertyName { get; set; } = ""Instance"";
    }
}
";


        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Post init - add attribute definitions
            context.RegisterPostInitializationOutput((i) =>
            {
                i.AddSource($"{AttributeFullName}.g.cs", AttributeCode);
            });
           
            // Syntax provider: find candidate class declarations
            var classSymbols = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds &&
                        !cds.Modifiers.Any(SyntaxKind.StaticKeyword) && cds.AttributeLists.Any(),
                    transform: (genCtx, ct) =>
                    {
                        var classDecl = (ClassDeclarationSyntax)genCtx.Node;
                        return genCtx.SemanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
                    })
                .Where(s => s != null)
                .Collect();

            // Combine with compilation so we can inspect referenced assemblies
            var compilationAndClasses = context.CompilationProvider.Combine(context.ParseOptionsProvider).Combine(classSymbols);


            context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
            {
                var ((compilation, parseOptions), classes) = source;
                if (classes.IsDefaultOrEmpty)
                {
                    return;
                }
                var codeWriter = new CodeWriter(parseOptions, compilation, spc);
                codeWriter.ForeachClassByInheritanceOrder(classes, ProcessClass);
            });
        }

        private CodeFile ProcessClass(INamedTypeSymbol classSymbol, CodeWriter codeWriter)
        {
            if (!classSymbol.HasAttribute(AttributeFullName))
            {
                return null;
            }
            if (classSymbol.Constructors.Any(p => p.DeclaringSyntaxReferences.Count() > 0))
            {
                codeWriter.AlreadyExistsEmptyConstructor(classSymbol);
                return null;
            }
            var instanceName = classSymbol.GetAttributes().Where(p => p.AttributeClass.Is(AttributeFullName))
               .Select(p => p.NamedArguments.Where(t => t.Key == PropertyName).Where(t => !t.Value.IsNull).Select(t => (string)t.Value.Value).FirstOrDefault())
               .FirstOrDefault();
            instanceName = string.IsNullOrEmpty(instanceName) ? "Instance" : instanceName;

            if (!IsValidName(instanceName))
            {
                codeWriter.InvalidInstancePropertyName(classSymbol, instanceName);
                return null;
            }

            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder("CS1591");
            AppendUsingLines(classSymbol, codeBuilder);
            AppendNamespace(classSymbol, codeBuilder);
            AppendClassDefinition(classSymbol, codeBuilder);
            AppendSingletonBody(classSymbol, instanceName, codeBuilder);
            codeBuilder.EndAllSegments();
            return new CodeFile
            {
                BasicName = classSymbol.GetCodeFileBasicName(),
                Content = codeBuilder.ToString(),
            };
        }
        bool IsValidName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, "^[_a-zA-Z][_a-zA-Z0-9]*$");
        }
        void AppendUsingLines(INamedTypeSymbol _, CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines("using System;");
        }

        void AppendNamespace(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            if (!classSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                codeBuilder.AppendCodeLines($"namespace {classSymbol.ContainingNamespace.ToDisplayString()}");
                codeBuilder.BeginSegment();
            }
        }
        void AppendClassDefinition(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            var classSymbols = classSymbol.GetContainerClassChains();
            foreach (var parentClass in classSymbols)
            {
                codeBuilder.AppendCodeLines($@"partial class {parentClass.GetClassSymbolDisplayText()}");
                codeBuilder.BeginSegment();
            }
        }

        void AppendSingletonBody(INamedTypeSymbol classSymbol, string instanceName, CsharpCodeBuilder codeBuilder)
        {


            var content = $@"private static readonly Lazy<{classSymbol.GetClassSymbolDisplayText()}> LazyInstance = new Lazy<{classSymbol.GetClassSymbolDisplayText()}>(() => new {classSymbol.GetClassSymbolDisplayText()}(), true);

private {classSymbol.Name}() {{ }}

public static {classSymbol.GetClassSymbolDisplayText()} {instanceName} => LazyInstance.Value;";

            codeBuilder.AppendCodeLines(content);
        }

        
    }
}
