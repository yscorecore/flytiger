using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.AutoNotify
{
    [Generator]
    class AutoNotifyGenerator : ISourceGenerator
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
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization((i) =>
            {
                i.AddSource($"{AttributeFullName}.g.cs", AttributeCode);
            });
            context.RegisterForSyntaxNotifications(() => new AutoNotifySyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is AutoNotifySyntaxReceiver receiver))
                return;

            var codeWriter = new CodeWriter(context);

            foreach (var clazzSymbol in codeWriter.GetAllClassSymbolsIgnoreRepeated(receiver.CandidateClasses))
            {
                var fieldList = clazzSymbol.GetAllInstanceFieldsByAttribute(AttributeFullName).ToList();
                if (fieldList.Any())
                {
                    var codeFile = ProcessClass(clazzSymbol, fieldList, codeWriter);
                    codeWriter.WriteCodeFile(codeFile);
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness",
            "RS1024:Compare symbols correctly", Justification = "<Pending>")]
        private CodeFile ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, CodeWriter codeWriter)
        {
            var compilation = codeWriter.Compilation;
            INamedTypeSymbol notifySymbol =
                compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

            var classSymbols = classSymbol.GetContainerClassChains();


            CsharpCodeBuilder codeBuilder = new CsharpCodeBuilder();

            var allNamespaces = new HashSet<string>();

            allNamespaces.Add("System.ComponentModel");

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


            if (!classSymbol.AllInterfaces.Contains(notifySymbol))
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

        class AutoNotifySyntaxReceiver : ISyntaxReceiver
        {
            public IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    var hasInstanceFieldWithAttribute = classDeclarationSyntax.Members.OfType<FieldDeclarationSyntax>()
                        .Any(HasInstanceFieldAndDefinedAttribute);
                    if (hasInstanceFieldWithAttribute)
                    {
                        CandidateClasses.Add(classDeclarationSyntax);
                    }
                }

                bool HasInstanceFieldAndDefinedAttribute(FieldDeclarationSyntax fieldDeclarationSyntax)
                {
                    return fieldDeclarationSyntax.AttributeLists.Any() &&
                           !fieldDeclarationSyntax.Modifiers.Any(SyntaxKind.StaticKeyword) &&
                           !fieldDeclarationSyntax.Modifiers.Any(SyntaxKind.ConstKeyword);
                }
            }
        }
    }
}
