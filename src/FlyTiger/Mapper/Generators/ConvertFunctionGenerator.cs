using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using static FlyTiger.Mapper.Generators.Utils;

namespace FlyTiger.Mapper.Generators
{
    internal class ConvertFunctionGenerator
    {

        public void AppendFunctions(CodeWriter codeWriter,
            CsharpCodeBuilder codeBuilder, List<ConvertMappingInfo> rootMappingInfos, IList<AttributeData> attributeDatas)
        {
            //System.Diagnostics.Debugger.Launch();
            foreach (var rootMappingInfo in rootMappingInfos)
            {
                if (ConvertMappingInfo.CanMappingSubObject(rootMappingInfo.SourceType,
                        rootMappingInfo.TargetType))
                {
                    try
                    {
                        var context = new MapperContext(codeWriter, codeBuilder, rootMappingInfo, attributeDatas);
                        new ConvertObjectGenerator().AppendFunctions(context);
                        new CopySingleObjectGenerator().AppendFunctions(context);
                        new CopyDictionaryGenerator().AppendFunctions(context);
                        new CopyCollectionGenerator().AppendFunctions(context);
                        new QueryableGenerator().AppendFunctions(context);
                    }
                    catch (Exception ex)
                    {
                        // TOTO report error
                    }
                }
                else
                {
                    // TOTO report error
                }
            }
        }




    }
}

