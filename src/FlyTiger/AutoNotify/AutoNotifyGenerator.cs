using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.AutoNotify
{
    [Generator]
    class AutoNotifyGenerator : IIncrementalGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        const string AttributeName = "AutoNotifyAttribute";
        const string PropertyName = "PropertyName";
        internal static string AttributeFullName = $"{NameSpaceName}.{AttributeName}";
        const string AttributeCode = @"using System;
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class AutoNotifyAttribute : Attribute
    {
        public string PropertyName { get; set; }
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

            bool HasInstanceFieldAndDefinedAttribute(FieldDeclarationSyntax fieldDeclarationSyntax)
            {
                return fieldDeclarationSyntax.AttributeLists.Any() &&
                       !fieldDeclarationSyntax.Modifiers.Any(SyntaxKind.StaticKeyword) &&
                       !fieldDeclarationSyntax.Modifiers.Any(SyntaxKind.ConstKeyword);
            }

            // Syntax provider: find candidate class declarations
            var classSymbols = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax cds &&
                        !cds.Modifiers.Any(SyntaxKind.StaticKeyword),
                    transform: (genCtx, ct) =>
                    {
                        var classDecl = (ClassDeclarationSyntax)genCtx.Node;
                        var hasInstanceFieldWithAttribute = classDecl.Members.OfType<FieldDeclarationSyntax>()
                             .Any(HasInstanceFieldAndDefinedAttribute);
                        if (hasInstanceFieldWithAttribute)
                        {
                            return genCtx.SemanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
                        }
                        else
                        {
                            return null;
                        }
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
                // codeWriter.ForeachClass(classes, (clazz,cw);
                foreach (var clazzSymbol in classes)
                {
                    var fieldList = clazzSymbol.GetAllInstanceFieldsByAttribute(AttributeFullName).ToList();
                    if (fieldList.Any())
                    {
                        var codeFile = ProcessClass(clazzSymbol, fieldList, codeWriter);
                        codeWriter.WriteCodeFile(codeFile);
                    }
                }

            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness",
            "RS1024:Compare symbols correctly", Justification = "<Pending>")]
        private CodeFile ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, CodeWriter codeWriter)
        {
            var compilation = codeWriter.Compilation;
            INamedTypeSymbol notifySymbol =
                compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

            var classSymbols = classSymbol.GetContainerClassChains();


            var codeBuilder = new CsharpCodeBuilder("CS0067");

            var allNamespaces = new HashSet<string>
            {
                "System.ComponentModel"
            };

            foreach (var usingNamespace in allNamespaces.OrderBy(p => p))
            {
                codeBuilder.AppendCodeLines($"using {usingNamespace};");
            }

            //codeBuilder.AppendCodeLines($@"using System.ComponentModel;");
            if (!classSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                codeBuilder.AppendCodeLines($"namespace {classSymbol.ContainingNamespace.ToDisplayString()}");
                codeBuilder.BeginSegment();
            }

            foreach (var parentClass in classSymbols)
            {
                if (parentClass != classSymbol)
                {
                    codeBuilder.AppendCodeLines($@"partial class {parentClass.GetClassSymbolDisplayText()}");
                    codeBuilder.BeginSegment();
                }
            }
            var shouldGeneratePropertyChangedEvent = !classSymbol.AllInterfaces.Contains(notifySymbol) && !BaseTypeHasDefinedFields(classSymbol.BaseType);


            if (shouldGeneratePropertyChangedEvent)
            {
                codeBuilder.AppendCodeLines(
                    $@"partial class {classSymbol.GetClassSymbolDisplayText()} : {notifySymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines("public event PropertyChangedEventHandler PropertyChanged;");
            }
            else
            {
                codeBuilder.AppendCodeLines($@"partial class {classSymbol.Name}");
                codeBuilder.BeginSegment();
            }

            // create properties for each field 
            foreach (IFieldSymbol fieldSymbol in fields)
            {
                ProcessField(codeBuilder, classSymbol, fieldSymbol, codeWriter);
            }

            codeBuilder.EndAllSegments();
            return new CodeFile
            {
                BasicName = classSymbol.GetCodeFileBasicName(),
                Content = codeBuilder.ToString(),
            };

            bool BaseTypeHasDefinedFields(INamedTypeSymbol namedTypeSymbol)
            {
                if (namedTypeSymbol == null)
                {
                    return false;
                }
                return namedTypeSymbol.AnyFieldsByAttribute(AttributeFullName) || BaseTypeHasDefinedFields(namedTypeSymbol.BaseType);
            }
        }

        private static void ProcessField(CsharpCodeBuilder source, INamedTypeSymbol classSymbol, IFieldSymbol fieldSymbol, CodeWriter codeWriter)
        {
            // get the name and type of the field
            string fieldName = fieldSymbol.Name;
            ITypeSymbol fieldType = fieldSymbol.Type;

            AttributeData attributeData = fieldSymbol.GetAttributes()
                .Single(p => p.AttributeClass.Is(AttributeFullName));
            // get the AutoNotify attribute from the field, and any associated data

            TypedConstant overridenNameOpt =
                attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == PropertyName).Value;

            string propertyName = NormalName(fieldName, overridenNameOpt);
            if (!IsValidName(propertyName))
            {
                codeWriter.InvalidPropertyName(fieldSymbol, propertyName);
                return;
            }

            if (classSymbol.GetAllMembers().Any(t => t.Name == propertyName))
            {
                codeWriter.PropertyNameAlreadyExists(classSymbol, fieldSymbol, propertyName);
                return;
            }

            source.AppendCodeLines($@"
public {fieldType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {propertyName} 
{{
    get 
    {{
        return this.{fieldName};
    }}
    set
    {{
        if(this.{fieldName} != value)
        {{
            this.{fieldName} = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propertyName})));
        }}
    }}
}}

");

            string NormalName(string field, TypedConstant overridenNameOption)
            {
                if (!overridenNameOption.IsNull)
                {
                    return overridenNameOption.Value.ToString();
                }

                field = field.TrimStart('_');
                if (field.Length == 0)
                    return string.Empty;

                if (field.Length == 1)
                    return field.ToUpperInvariant();

                return field.Substring(0, 1).ToUpperInvariant() + field.Substring(1);
            }
            bool IsValidName(string name)
            {
                return System.Text.RegularExpressions.Regex.IsMatch(name, "^[_a-zA-Z][_a-zA-Z0-9]*$");
            }
        }


    }
}
