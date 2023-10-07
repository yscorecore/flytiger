using System;
using Microsoft.CodeAnalysis;

namespace FlyTiger
{

    internal class CopyToMethodInfo : IEquatable<CopyToMethodInfo>
    {
        public bool IsCollection { get; set; }
        public ITypeSymbol SourceType { get => Context.MappingInfo.SourceType; }
        public ITypeSymbol TargetType { get => Context.MappingInfo.TargetType; }
        public MapperContext Context { get; set; }

        public bool Equals(CopyToMethodInfo other)
        {
            return this.IsCollection == other.IsCollection
                 && this.SourceType.Equals(other.SourceType, SymbolEqualityComparer.Default)
                 && this.TargetType.Equals(other.TargetType, SymbolEqualityComparer.Default);
        }
    }

}
