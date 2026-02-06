using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FlyTiger.Generator.UnitTest
{
    public class BaseGeneratorTest
    {
        protected void ShouldGenerateExpectCodeFile(IIncrementalGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            ShouldGenerateExpectCodeFile(generator.AsSourceGenerator(), testCaseFileName, assemblies);
        }

        protected void ShouldGenerateExpectCodeFile(ISourceGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            XDocument xmlFile = XDocument.Load(Path.Combine("TestCases", testCaseFileName));
            var codes = xmlFile.XPathSelectElements("case/input/code")
                .Select(prop => prop.Value).ToArray();
            var newComp = RunGenerators(CreateCompilation(codes, assemblies), out var warningAndErrors,
                generator);

            // warningAndErrors.Should().BeEmpty();

            var outputs = xmlFile.XPathSelectElements("case/output/code")
                .Select(prop => (File: prop.Attribute("file").Value, Content: prop.Value));

            foreach (var output in outputs)
            {
                newComp.SyntaxTrees.Should()
                    .ContainSingle(x => Path.GetFileName(x.FilePath).Equals(output.File),
                        $"output file '{output.File}' not generated")
                    .Which.GetText().ToString().NormalizeCode().Should().BeEquivalentTo(output.Content.NormalizeCode());
            }
        }

        public static string GetProjectRootDirectory()
        {
            // 获取当前执行测试程序集的位置
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                return Directory.GetCurrentDirectory();
            }

            // 向上查找直到找到 .csproj 文件
            var directoryInfo = new DirectoryInfo(assemblyDirectory);

            while (directoryInfo != null)
            {
                // 查找 .csproj 文件
                var csprojFiles = Directory.GetFiles(directoryInfo.FullName, "*.csproj");
                if (csprojFiles.Length > 0)
                {
                    return directoryInfo.FullName;
                }

                // 也可以查找 .sln 文件作为备选
                var slnFiles = Directory.GetFiles(directoryInfo.FullName, "*.sln");
                if (slnFiles.Length > 0)
                {
                    return directoryInfo.FullName;
                }

                directoryInfo = directoryInfo.Parent;
            }

            return assemblyDirectory;
        }
        protected void UpdateTestOutput(IIncrementalGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            UpdateTestOutput(generator.AsSourceGenerator(), testCaseFileName, assemblies);
        }
        /// <summary>
        /// 临时方法，用于更新测试用例的 output 节点内容
        /// </summary>
        protected void UpdateTestOutput(ISourceGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            var testCasePath = Path.Combine(GetProjectRootDirectory(), Path.Combine("TestCases", testCaseFileName));
            XDocument xmlFile = XDocument.Load(testCasePath);
            var codes = xmlFile.XPathSelectElements("case/input/code")
                .Select(prop => prop.Value).ToArray();
            var newComp = RunGenerators(CreateCompilation(codes, assemblies), out var warningAndErrors,
                generator);



            var outputElement = xmlFile.XPathSelectElement("case/output");
            var outputElements = outputElement.XPathSelectElements("code").ToList();

            outputElements.ForEach(p => p.Remove());



            var outputFiles = newComp.SyntaxTrees.Where(p => p.FilePath.EndsWith("g.cs"))
                .Select(p => new
                {
                    file = Path.GetFileName(p.FilePath),
                    content = p.GetText().ToString().NormalizeCode()
                }).ToList();

            outputFiles.ForEach(p =>
            {
                outputElement.Add(new XElement("code", new XAttribute("file", p.file), new XCData(p.content)));
            });



            // 保存更新后的 XML 文件
            xmlFile.Save(testCasePath);
        }

        protected void ShouldReportDiagnostic(IIncrementalGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            ShouldReportDiagnostic(generator.AsSourceGenerator(), testCaseFileName, assemblies);
        }
        protected void ShouldReportDiagnostic(ISourceGenerator generator, string testCaseFileName, params Assembly[] assemblies)
        {
            XDocument xmlFile = XDocument.Load(Path.Combine("TestCases", testCaseFileName));
            var codes = xmlFile.XPathSelectElements("case/input/code")
                .Select(prop => prop.Value).ToArray();
            var newComp = RunGenerators(CreateCompilation(codes, assemblies), out var warningAndErrors,
                generator);

            var outputs = xmlFile.XPathSelectElements("case/output/diagnostic")
                .Select(prop => (Code: prop.Attribute("code").Value, Message: prop.Value.Trim()));
            foreach (var output in outputs)
            {
                var dig = warningAndErrors.FirstOrDefault(p => p.Id == output.Code);
                dig.Should().NotBeNull($"missing expected diagnostic, code:{output.Code}");
                dig.GetMessage().Should().Match(output.Message);
            }

        }

        private static Compilation CreateCompilation(string[] sources, Assembly[] assemblies)
        {
            var allSources = sources
                .Select(p => CSharpSyntaxTree.ParseText(p));
            var allReferenceAssemblies =
                assemblies.Select(p => MetadataReference.CreateFromFile(p.Location));
            var defaultAssemblies = new MetadataReference[]
                {
                    MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
                };
            return CSharpCompilation.Create("tempassembly",
                allSources,
                defaultAssemblies.Union(allReferenceAssemblies),
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }

        private static GeneratorDriver CreateDriver(params ISourceGenerator[] generators)
            => CSharpGeneratorDriver.Create(generators);

        private static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics,
            params ISourceGenerator[] generators)
        {
            CreateDriver(generators)
                .RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out diagnostics);
            return newCompilation;
        }
    }

}
