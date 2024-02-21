using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper
{

    internal class CopyToMethodInfo : IEquatable<CopyToMethodInfo>
    {

        public CopyToMethodInfo(CopyToMethodType methodType, MapperContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            CopyMethodType = methodType;
            Context = context;
            this.InlineMethodName = methodType == CopyToMethodType.ConvertSingle ?
                BuildConvertObjectMethodName(context.MappingInfo.SourceType, context.MappingInfo.TargetType) :
                BuildCopyObjectMethodName(context.MappingInfo.SourceType, context.MappingInfo.TargetType);

        }
        private static string BuildCopyObjectMethodName(ITypeSymbol source, ITypeSymbol target)
        {
            var sourceName = new string(source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            var targetName = new string(target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return $"Copy{sourceName}To{targetName}";
        }
        private static string BuildConvertObjectMethodName(ITypeSymbol source, ITypeSymbol target)
        {
            var sourceName = new string(source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            var targetName = new string(target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return $"Convert{sourceName}To{targetName}";
        }

        public bool IsCollection { get; private set; }
        public bool IsDictionary { get; private set; }
        public CopyToMethodType CopyMethodType { get; private set; }
        public ITypeSymbol SourceType { get => Context.MappingInfo.SourceType; }
        public ITypeSymbol TargetType { get => Context.MappingInfo.TargetType; }
        public string InlineMethodName { get; private set; }
        public MapperContext Context { get; private set; }

        public bool Equals(CopyToMethodInfo other)
        {
            return this.CopyMethodType == other.CopyMethodType
                 && this.SourceType.Equals(other.SourceType, SymbolEqualityComparer.Default)
                 && this.TargetType.Equals(other.TargetType, SymbolEqualityComparer.Default);
        }
    }
    internal enum CopyToMethodType
    {
        CopySingle,
        CopyCollection,
        CopyDictionary,
        ConvertSingle,
    }

}
