using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.AutoConstructor
{
    [Generator]
    class AutoConstructorGenerator : IIncrementalGenerator
    {
        const string NameSpaceName = nameof(FlyTiger);
        const string AttributeName = "AutoConstructorAttribute";
        const string IgnoreAttributeName = "AutoConstructorIgnoreAttribute";
        const string InitializeAttributeName = "AutoConstructorInitializeAttribute";
        const string NullCheckPropertyName = "NullCheck";
        static readonly string AttributeFullName = $"{NameSpaceName}.{AttributeName}";
        static readonly string IgnoreAttributeFullName = $"{NameSpaceName}.{IgnoreAttributeName}";
        internal static readonly string InitializeAttributeFullName = $"{NameSpaceName}.{InitializeAttributeName}";
        const string AttributeCode = @"using System;
namespace FlyTiger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class AutoConstructorAttribute : Attribute
    {
        public bool NullCheck { get; set; } = false;
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class AutoConstructorIgnoreAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class AutoConstructorInitializeAttribute : Attribute
    {
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
                var ((compilation, parseOptions),classes) = source;
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

            var nameMapper = GetSymbolNameMapper(classSymbol);

            if (!nameMapper.Any() && !HasDefineValidAutoConstructorInitializeAttribute(classSymbol))
            {
                return null;
            }
            var isDependencyInjection = codeWriter.Compilation.ReferencedAssemblyNames
                    .Any(p => p.Name == "Microsoft.Extensions.DependencyInjection.Abstractions");

            var nullChecked = GetNullCheckValue(classSymbol);



            CsharpCodeBuilder builder = new CsharpCodeBuilder("CS1591");
            AppendNamespace(classSymbol, builder);
            AppendClassDefinition(classSymbol, builder);
            AppendPublicCtor(classSymbol, nameMapper, isDependencyInjection, nullChecked, builder, codeWriter);

            builder.EndAllSegments();
            return new CodeFile
            {
                BasicName = classSymbol.GetCodeFileBasicName(),
                Content = builder.ToString(),
            };

        }
        bool GetNullCheckValue(INamedTypeSymbol classSymbol)
        {
            var attr = classSymbol.GetAttributes()
                .Where(p => p.AttributeClass.Is(AttributeFullName))
                .FirstOrDefault();
            if (attr != null)
            {
                var val = attr.NamedArguments.Where(p => p.Key == NullCheckPropertyName)
                     .Select(p => p.Value.Value).FirstOrDefault();
                if (val != null)
                {
                    return Convert.ToBoolean(val);
                }
            }
            return false;
        }

        bool HasDefineValidAutoConstructorInitializeAttribute(INamedTypeSymbol classSymbol)
        {
            return classSymbol.GetMembers().OfType<IMethodSymbol>()
                  .Any(p => p.Parameters.Length == 0 && p.HasAttribute(InitializeAttributeFullName));
        }
        IDictionary<string, ArgumentInfo> GetSymbolNameMapper(INamedTypeSymbol classSymbol)
        {
            var nameMapper = new Dictionary<string, ArgumentInfo>();

            if (classSymbol.BaseType != null && classSymbol.BaseType.HasAttribute(AttributeFullName))
            {
                foreach (var kv in GetSymbolNameMapper(classSymbol.BaseType))
                {
                    var newName = NewArgumentName(kv.Value.ArgName, nameMapper);
                    nameMapper[kv.Key] = new ArgumentInfo
                    {
                        ArgName = newName,
                        ArgTypeSymbol = kv.Value.ArgTypeSymbol,
                        MemberName = newName,
                        MemberTypeSymbol = kv.Value.MemberTypeSymbol,
                        Source = ArgumentSource.BaseCtor
                    };
                }
            }
            else
            {
                foreach (var baseParam in GetBaseTypeParameters())
                {

                    var newName = NewArgumentName(baseParam.Name.ToCamelCase(), nameMapper);
                    nameMapper[newName] = new ArgumentInfo
                    {
                        ArgName = newName,
                        ArgTypeSymbol = baseParam.Type,
                        MemberName = baseParam.Name,
                        MemberTypeSymbol = baseParam.Type,
                        Source = ArgumentSource.BaseCtor
                    };
                }
            }

               
            foreach (var field in GetInstanceFields())
            {
                var newName = NewArgumentName(field.Name.ToCamelCase(), nameMapper);
                nameMapper[newName] = new ArgumentInfo
                {
                    ArgName = newName,
                    ArgTypeSymbol = field.Type,
                    MemberName = field.Name,
                    MemberTypeSymbol = field.Type,
                    Source = ArgumentSource.Field
                };
            }
            foreach (var property in GetInstanceProperties())
            {
                var newName = NewArgumentName(property.Name.ToCamelCase(), nameMapper);
                nameMapper[newName] = new ArgumentInfo
                {
                    ArgName = newName,
                    ArgTypeSymbol = property.Type,
                    MemberName = property.Name,
                    MemberTypeSymbol = property.Type,
                    Source = ArgumentSource.Property
                };
            }


            return nameMapper;

            IEnumerable<IFieldSymbol> GetInstanceFields()
            {
                return classSymbol.GetMembers().OfType<IFieldSymbol>()
                    .Where(p => !p.IsStatic && !p.IsConst && p.CanBeReferencedByName && !p.HasAttribute(IgnoreAttributeFullName));
            }
            IEnumerable<IPropertySymbol> GetInstanceProperties()
            {
                return classSymbol.GetMembers().OfType<IPropertySymbol>()
                    .Where(p => !p.IsStatic && !p.IsIndexer && p.CanBeReferencedByName && p.IsAutoProperty() && !p.HasAttribute(IgnoreAttributeFullName));
            }

            ImmutableArray<IParameterSymbol> GetBaseTypeParameters()
            {
                if (classSymbol.BaseType == null)
                {
                    return ImmutableArray<IParameterSymbol>.Empty;
                }

                // if found Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructorAttribute, use it first
                var primaryCtor = classSymbol.BaseType.Constructors
                    .FirstOrDefault(p => p.HasAttribute(
                        "Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructorAttribute"));
                if (primaryCtor != null)
                {
                    return primaryCtor.Parameters;
                }

                // use first minimum parameters
                var ctor = classSymbol.BaseType.Constructors.OrderBy(method => method.Parameters.Length)
                    .FirstOrDefault();
                if (ctor == null)
                {
                    return ImmutableArray<IParameterSymbol>.Empty;
                }

                return ctor.Parameters;
            }

            string NewArgumentName(string baseName, IDictionary<string, ArgumentInfo> ctx)
            {
                if (string.IsNullOrEmpty(baseName))
                {
                    baseName = "args";
                }

                string newName = baseName;
                int index = 1;
                while (ctx.ContainsKey(newName))
                {
                    newName = baseName + index++;
                }

                return newName;
            }
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
        void AppendPublicCtor(INamedTypeSymbol classSymbol, IDictionary<string, ArgumentInfo> nameMapper, bool isDependencyInjection, bool nullCheck, CsharpCodeBuilder codeBuilder,CodeWriter codeWriter)
        {
            if (isDependencyInjection)
            {
                codeBuilder.AppendCodeLines("[Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]");
            }

            string args = string.Join(", ", nameMapper.Select(p => p.Value.GetCtorMethodArgumentItem()));


            codeBuilder.AppendCodeLines($"public {classSymbol.Name}({args})");
            var baseCtorArgs = nameMapper.Values.Where(p => p.Source == ArgumentSource.BaseCtor);
            if (baseCtorArgs.Any())
            {
                string baseArgs = string.Join(", ", baseCtorArgs.Select(p => $"{p.MemberName}: {p.ArgName}"));
                codeBuilder.AppendCodeLines($"    : base({baseArgs})");
            }


            codeBuilder.BeginSegment();
            // lines
            foreach (var member in nameMapper.Values.Where(p => p.Source != ArgumentSource.BaseCtor))
            {
                codeBuilder.AppendCodeLines(BuildCtorAssignLine(member));
            }
            // init methods
            var allInitializeMethods = GetAllInitializeMethods().ToList();
            if (allInitializeMethods.Count > 1)
            {
                codeWriter.InitializeMethodShouldOnlyOne(classSymbol, allInitializeMethods);
            }
            foreach (var method in allInitializeMethods)
            {
                if (method.Parameters.Any())
                {
                    codeWriter.InitializeMethodShouldHasNoneArguments(method);
                    // compile error
                    continue;
                }

                if (method.IsStatic)
                {
                    codeWriter.InitializeMethodShouldNotBeStatic(method);
                    codeBuilder.AppendCodeLines(BuildInitializeMethod(method));
                    continue;
                }
                if (!method.ReturnsVoid)
                {
                    codeWriter.InitializeMethodShouldReturnVoid(method);
                    codeBuilder.AppendCodeLines(BuildInitializeMethod(method));
                    continue;
                }
                codeBuilder.AppendCodeLines(BuildInitializeMethod(method));
            }

            codeBuilder.EndSegment();

            string BuildCtorAssignLine(ArgumentInfo argumentInfo)
            {
                if (nullCheck && !argumentInfo.ArgTypeSymbol.IsValueType)
                {
                    return $"this.{argumentInfo.MemberName} = {argumentInfo.ArgName} ?? throw new System.ArgumentNullException(nameof({argumentInfo.ArgName}));";
                }
                else
                {
                    return $"this.{argumentInfo.MemberName} = {argumentInfo.ArgName};";
                }
            }
            string BuildInitializeMethod(IMethodSymbol method)
            {
                if (method.IsStatic)
                {
                    return $"{method.Name}();";
                }
                else
                {
                    return $"this.{method.Name}();";
                }
            }
            IEnumerable<IMethodSymbol> GetAllInitializeMethods()
            {
                return classSymbol.GetAllMethodsByAttribute(InitializeAttributeFullName);
            }

        }



        private class ArgumentInfo
        {
            public string ArgName { get; set; }
            public ITypeSymbol ArgTypeSymbol { get; set; }

            public string MemberName { get; set; }

            public ITypeSymbol MemberTypeSymbol { get; set; }

            public ArgumentSource Source { get; set; }

            public string GetCtorMethodArgumentItem()
            {
                return $"{ArgTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {ArgName}";
            }
        }
        private enum ArgumentSource
        {
            BaseCtor,
            Field,
            Property
        }

       
    }
}
