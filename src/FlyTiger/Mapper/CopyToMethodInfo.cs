using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlyTiger.Mapper
{

    internal class CopyToMethodInfo : IEquatable<CopyToMethodInfo>
    {
        public CopyToMethodInfo(bool isCollection, MapperContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            IsCollection = isCollection;
            Context = context;
            this.CopyToMethodName = BuildCopyObjectMethodName(context.MappingInfo.SourceType, context.MappingInfo.TargetType);

        }
        private static string BuildCopyObjectMethodName(ITypeSymbol source, ITypeSymbol target)
        {
            var sourceName = new string(source.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            var targetName = new string(target.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return $"Copy{sourceName}To{targetName}";
        }

        public bool IsCollection { get; private set; }
        public ITypeSymbol SourceType { get => Context.MappingInfo.SourceType; }
        public ITypeSymbol TargetType { get => Context.MappingInfo.TargetType; }
        public string CopyToMethodName { get; private set; }
        public MapperContext Context { get; private set; }

        public bool Equals(CopyToMethodInfo other)
        {
            return this.IsCollection == other.IsCollection
                 && this.SourceType.Equals(other.SourceType, SymbolEqualityComparer.Default)
                 && this.TargetType.Equals(other.TargetType, SymbolEqualityComparer.Default);
        }
    }

}
