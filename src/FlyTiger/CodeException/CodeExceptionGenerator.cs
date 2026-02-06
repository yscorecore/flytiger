using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.CodeException
{
    [Generator]
    internal class CodeExceptionGenerator : IIncrementalGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        const string AttributeName = "CodeExceptionsAttribute";
        const string ItemAttributeName = "CodeExceptionAttribute";
        const string ResourceBaseNamePropertyName = "ResourceBaseName";
        const string CodePrefixPropertyName = "CodePrefix";
        const string CustomTemplatePropertyName = "CustomTemplate";
        const string IncludeExceptionDataPropertyName = "IncludeExceptionData";
        internal static string AttributeFullName = $"{NameSpaceName}.{AttributeName}";
        internal static string ItemAttributeFullName = $"{NameSpaceName}.{ItemAttributeName}";
        public void Execute(GeneratorExecutionContext context)
        {
            //if (!(context.SyntaxReceiver is CodeExceptionSyntaxReceiver receiver))
            //    return;
            //var codeWriter = new CodeWriter(context);



            //codeWriter.ForeachClassSyntax(receiver.CandidateClasses, ProcessClass);
        }


        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Post init - add attribute definitions
            context.RegisterPostInitializationOutput((i) =>
            {
                i.AddSource($"{nameof(Code.CodeExceptionsAttribute)}.g.cs", Code.CodeExceptionsAttribute);
                i.AddSource($"{nameof(Code.CodeExceptionAttribute)}.g.cs", Code.CodeExceptionAttribute);
                i.AddSource($"{nameof(Code.TextValuesFormatter)}.g.cs", Code.TextValuesFormatter);
                i.AddSource($"{nameof(Code.CodeException)}.g.cs", Code.CodeException);
            });
            // Syntax provider: find candidate class declarations
            var classSymbols = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds &&
                         cds.AttributeLists.Any(),
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
            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder("CS1591", "CS0219");
            AppendUsingLines(classSymbol, codeBuilder);
            AppendNamespace(classSymbol, codeBuilder);
            AppendClassDefinition(classSymbol, codeBuilder);
            AppendResourceManager(classSymbol, codeBuilder);
            AppendGetErrorMessage(classSymbol, codeBuilder);
            AppendGetErrorCode(classSymbol, codeBuilder);
            AppendPropertyException(classSymbol, codeBuilder);
            AppendMethodException(classSymbol, codeBuilder);
            codeBuilder.EndAllSegments();
            return new CodeFile
            {
                BasicName = classSymbol.GetCodeFileBasicName(),
                Content = codeBuilder.ToString(),
            };
        }
        void AppendUsingLines(INamedTypeSymbol _, CsharpCodeBuilder codeBuilder)
        {
            codeBuilder.AppendCodeLines("using System;");
            codeBuilder.AppendCodeLines("using System.Linq;");
            codeBuilder.AppendCodeLines("using System.Resources;");
            codeBuilder.AppendCodeLines("using System.Collections.Generic;");
            codeBuilder.AppendCodeLines("using FlyTiger.CodeException;");
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
        void AppendResourceManager(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            string code = @"static readonly Lazy<ResourceManager> __resourceManager = new Lazy<ResourceManager>(() =>
{
    var assembly = typeof(${className}).Assembly;
    var baseName = ${baseName};
    baseName = string.IsNullOrEmpty(baseName) ? typeof(${className}).FullName : baseName;
    var res = assembly.GetManifestResourceInfo($""{baseName}.resources"");
    return res == null ? null : new ResourceManager(baseName, assembly);
}, true);";
            var baseName = classSymbol.GetAttributes().Where(p => p.AttributeClass.Is(AttributeFullName))
              .Select(p => p.NamedArguments.Where(t => t.Key == ResourceBaseNamePropertyName).Where(t => !t.Value.IsNull).Select(t => (string)t.Value.Value).FirstOrDefault())
              .FirstOrDefault();
            code = code.Replace("${className}", classSymbol.GetClassSymbolDisplayText());
            code = code.Replace("${baseName}", baseName.Repr());
            codeBuilder.AppendCodeLines(code);
            codeBuilder.AppendLine();
        }
        void AppendGetErrorMessage(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            var code = @"private static string __GetErrorMessage(string name, string defaultMessage, object[] args, Dictionary<string, object> kwArgs)
{
    var resMan = __resourceManager.Value;
    var template = resMan?.GetString(name) ?? defaultMessage ?? string.Empty;
    if (args == null || args.Length == 0)
    {
        return template;
    }
    else
    {
        var valueFormatter = TextValuesFormatter.FromText(template);
        return valueFormatter.Format(args, kwArgs);
    }
}";
            codeBuilder.AppendCodeLines(code);
            codeBuilder.AppendLine();
        }
        void AppendGetErrorCode(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            var code = @"private static string __GetErrorCode(string code)
{
    return ${prefix} + code;
}";
            var prefix = classSymbol.GetAttributes().Where(p => p.AttributeClass.Is(AttributeFullName))
              .Select(p => p.NamedArguments.Where(t => t.Key == CodePrefixPropertyName).Where(t => !t.Value.IsNull).Select(t => (string)t.Value.Value).FirstOrDefault())
              .FirstOrDefault();
            code = code.Replace("${prefix}", (prefix ?? string.Empty).Repr());
            codeBuilder.AppendCodeLines(code);
            codeBuilder.AppendLine();
        }



        void AppendPropertyException(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            var properties = classSymbol.GetAllPropertiesByAttribute(ItemAttributeFullName);
            foreach (var property in properties)
            {
                AppendPropertyException(classSymbol, property, codeBuilder);
            }
        }
        void AppendPropertyException(INamedTypeSymbol classSymbol, IPropertySymbol propertySymbol, CsharpCodeBuilder codeBuilder)
        {

        }
        void AppendMethodException(INamedTypeSymbol classSymbol, CsharpCodeBuilder codeBuilder)
        {
            var appendData = classSymbol.GetAttributes().Where(p => p.AttributeClass.Is(AttributeFullName))
             .Select(p => p.NamedArguments.Where(t => t.Key == IncludeExceptionDataPropertyName).Where(t => !t.Value.IsNull).Select(t => (bool?)t.Value.Value).FirstOrDefault())
             .FirstOrDefault() ?? true;
            var exceptionExpression = classSymbol.GetAttributes().Where(p => p.AttributeClass.Is(AttributeFullName))
              .Select(p => p.NamedArguments.Where(t => t.Key == CustomTemplatePropertyName).Where(t => !t.Value.IsNull).Select(t => (string)t.Value.Value).FirstOrDefault())
              .FirstOrDefault() ?? "new CodeException(__code, __message, __innerException)";
            var exceptionInfo = new ExceptionInfo
            {
                AppendData = appendData,
                Expression = exceptionExpression
            };

            var methods = classSymbol.GetAllMethodsByAttribute(ItemAttributeFullName);
            foreach (var method in methods)
            {
                AppendMethodException(classSymbol, method, exceptionInfo, codeBuilder);
            }

        }

        void AppendMethodException(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol, ExceptionInfo exceptionInfo, CsharpCodeBuilder codeBuilder)
        {
            var codeInfo = methodSymbol.GetAttributes().Where(p => p.AttributeClass.Is(ItemAttributeFullName))
            .Select(p => new
            {
                Code = p.ConstructorArguments.First().Value?.ToString(),
                Message = p.ConstructorArguments.Last().Value?.ToString()
            }).Single();

            var allArgumentNames = methodSymbol.Parameters.Select(p => p.Name).ToList();
            var methodReturnType = methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var methodName = methodSymbol.Name;
            var argumentList = string.Join(", ", methodSymbol.Parameters.Select(p => $"{p.Type.ToDisplayString((SymbolDisplayFormat.FullyQualifiedFormat))} {p.Name}"));
            var innerException = methodSymbol.Parameters.Where(p => p.Type.Is(typeof(Exception).FullName)).SingleOrDefault();
            var methodModifier = methodSymbol.IsStatic ? $"{methodSymbol.GetAccessibilityText()} static" : methodSymbol.GetAccessibilityText();

            codeBuilder.AppendCodeLines($"{methodModifier} partial {methodReturnType} {methodName}({argumentList})");
            codeBuilder.BeginSegment();
            if (allArgumentNames.Count > 0)
            {
                codeBuilder.AppendCodeLines($"var __args = new object[] {{ {string.Join(", ", allArgumentNames)} }};");
            }
            else
            {
                codeBuilder.AppendCodeLines("var __args = Array.Empty<object>();");
            }

            codeBuilder.AppendCodeLines("var __kwargs = new Dictionary<string, object>");
            codeBuilder.BeginSegment();
            foreach (var name in allArgumentNames)
            {
                codeBuilder.AppendCodeLines($"[nameof({name})] = {name},");
            }
            codeBuilder.EndSegment("};");
            if (innerException != null)
            {
                codeBuilder.AppendCodeLines($"var __innerException = {innerException.Name};");
            }
            else
            {
                codeBuilder.AppendCodeLines($"var __innerException = default(Exception);");
            }
            codeBuilder.AppendCodeLines($"var __code = __GetErrorCode({codeInfo.Code.Repr()});");
            codeBuilder.AppendCodeLines($"var __message = __GetErrorMessage(nameof({methodSymbol.Name}), {codeInfo.Message.Repr()}, __args, __kwargs);");
            codeBuilder.AppendCodeLines($"var __exception = {exceptionInfo.Expression};");
            if (exceptionInfo.AppendData)
            {
                codeBuilder.AppendCodeLines("__kwargs.Where(p => !(p.Value is Exception)).ToList().ForEach(p => __exception.Data[p.Key] = p.Value);");
            }
            codeBuilder.AppendCodeLines("return __exception;");
            codeBuilder.EndSegment();




        }


        public class ExceptionInfo
        {
            public bool AppendData { get; set; }
            public string Expression { get; set; }

        }
    }
}
