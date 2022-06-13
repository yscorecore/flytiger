using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FlyTiger
{
    [Generator]
    class UsingFlyTigerGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var csharpOptions = context.ParseOptions as CSharpParseOptions;

            if (csharpOptions != null && (int)csharpOptions.LanguageVersion > 900)
            {
                //csharp 10 support global using
                var codeWriter = new CodeWriter(context);
                codeWriter.WriteCodeFile(new CodeFile
                {
                    BasicName = "GlobalUsing",
                    Content = "global using FlyTiger;"
                });
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
