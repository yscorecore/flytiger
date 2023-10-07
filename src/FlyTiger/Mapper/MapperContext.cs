using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger
{

    class MapperContext
    {
        public MapperContext(Compilation compilation, CsharpCodeBuilder codeBuilder,
            ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
        {
            this.Compilation = compilation;
            this.MappingInfo = convertMappingInfo;
            this.CodeBuilder = codeBuilder;
            this.AttributeLists = attributeList;
            this.WorkedPaths.Add(convertMappingInfo.TargetType);
        }

        private MapperContext(MapperContext baseConvertContext, ConvertMappingInfo convertMappingInfo, IList<AttributeData> attributeList)
            : this(baseConvertContext.Compilation, baseConvertContext.CodeBuilder,
                convertMappingInfo, attributeList)
        {
            this.AttributeLists = this.AttributeLists;
            this.WorkedPaths = new List<ITypeSymbol>(baseConvertContext.WorkedPaths);
            this.WorkedPaths.Add(convertMappingInfo.TargetType);
        }

        public Compilation Compilation { get; }
        public ConvertMappingInfo MappingInfo { get; }
        public CsharpCodeBuilder CodeBuilder { get; }
        public List<ITypeSymbol> WorkedPaths { get; } = new List<ITypeSymbol>();
        public IList<AttributeData> AttributeLists { get; } = new List<AttributeData>();
        public bool HasWalked(ITypeSymbol symbol)
        {
            foreach (var path in WorkedPaths)
            {
                if (path.Equals(symbol, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }

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
                    return new ConvertMappingInfo
                    {
                        SourceType = source,
                        TargetType = target,
                        CustomerMappings = new Dictionary<string, string>(),
                        IgnoreTargetProperties = new HashSet<string>()
                    };
                }
            }
        }
    }

}
