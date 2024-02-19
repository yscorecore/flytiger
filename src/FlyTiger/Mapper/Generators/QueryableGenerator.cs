using System;
using System.Collections.Generic;
using System.Text;

namespace FlyTiger.Mapper.Generators
{
    internal class QueryableGenerator : ConvertObjectGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toType = mappingInfo.TargetType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;

            if (!mappingInfo.MapQuery) return;
            codeBuilder.AppendCodeLines(
                $"private static IQueryable<{toTypeDisplay}> {mappingInfo.ConvertToMethodName}(this IQueryable<{fromTypeDisplay}> source)");
            codeBuilder.BeginSegment();
            codeBuilder.AppendCodeLines($"return source?.Select(p => new {toTypeDisplay}");
            codeBuilder.BeginSegment();
            AppendPropertyAssign("p", null, ",", context);
            codeBuilder.EndSegment("}).RebuildWithIncludeForEfCore();");
            codeBuilder.EndSegment();
        }
    }
}
