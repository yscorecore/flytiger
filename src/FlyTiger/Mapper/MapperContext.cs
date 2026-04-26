using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{

    class MapperContext
    {
        public MapperContext(CodeWriter codeWriter, CsharpCodeBuilder codeBuilder,
            ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
        {
            this.CodeWriter = codeWriter;
            this.MappingInfo = convertMappingInfo;
            this.CodeBuilder = codeBuilder;
            this.AttributeLists = attributeList;
            this.WorkedPaths.Add((convertMappingInfo.SourceType, convertMappingInfo.TargetType, convertMappingInfo.ConvertToMethodName));
        }

        private MapperContext(MapperContext baseConvertContext, ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
            : this(baseConvertContext.CodeWriter, baseConvertContext.CodeBuilder,
                convertMappingInfo, attributeList)
        {
            this.AttributeLists = this.AttributeLists;
            this.WorkedPaths = new List<(ITypeSymbol, ITypeSymbol, string)>(baseConvertContext.WorkedPaths);
            this.WorkedPaths.Add((convertMappingInfo.SourceType, convertMappingInfo.TargetType, convertMappingInfo.ConvertToMethodName));
        }
        public CodeWriter CodeWriter { get; }
        public Compilation Compilation { get => this.CodeWriter.Compilation; }
        public ConvertMappingInfo MappingInfo { get; }
        public CsharpCodeBuilder CodeBuilder { get; }
        public List<(ITypeSymbol Source, ITypeSymbol Target, string ConvertToMethodName)> WorkedPaths { get; } = new List<(ITypeSymbol, ITypeSymbol, string)>();
        public IList<AttributeData> AttributeLists { get; } = new List<AttributeData>();
        public bool HasWalked(ITypeSymbol source, ITypeSymbol target)
        {
            foreach (var path in WorkedPaths)
            {
                if (path.Source.Equals(source, SymbolEqualityComparer.Default) &&
                    path.Target.Equals(target, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasWalked(ITypeSymbol target)
        {
            foreach (var path in WorkedPaths)
            {
                if (path.Target.Equals(target, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Try to get the conversion method name for a previously walked (source, target) pair.
        /// Used for handling self-referencing (circular) properties by calling the existing method.
        /// </summary>
        public bool TryGetConvertMethodName(ITypeSymbol source, ITypeSymbol target, out string convertToMethodName)
        {
            foreach (var path in WorkedPaths)
            {
                if (path.Source.Equals(source, SymbolEqualityComparer.Default) &&
                    path.Target.Equals(target, SymbolEqualityComparer.Default) &&
                    !string.IsNullOrEmpty(path.ConvertToMethodName))
                {
                    convertToMethodName = path.ConvertToMethodName;
                    return true;
                }
            }
            convertToMethodName = null;
            return false;
        }

        public MapperContext Fork(ITypeSymbol source, ITypeSymbol target)
        {


            var newMappingInfo = CreateMappingInfo();
            return new MapperContext(this, newMappingInfo, this.AttributeLists);

            ConvertMappingInfo CreateMappingInfo()
            {
                var attributeData = this.AttributeLists
                   .Where(p => (p.ConstructorArguments.First().Value as INamedTypeSymbol).Equals(source,
                       SymbolEqualityComparer.Default))
                   .Where(p => (p.ConstructorArguments.Last().Value as INamedTypeSymbol).Equals(target,
                       SymbolEqualityComparer.Default))
                   .FirstOrDefault();
                if (attributeData != null)
                {
                    return ConvertMappingInfo.FromAttributeData(attributeData);
                }
                else
                {
                    return this.MappingInfo.Fork(source, target);
                }
            }
        }
    }

}
