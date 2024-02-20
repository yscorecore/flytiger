namespace FlyTiger.Mapper.Generators
{
    internal class QueryableGenerator : ConvertObjectGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;

            if (!mappingInfo.MapQuery) return;
            codeBuilder.AppendCodeLines(
                $"private static IQueryable<{mappingInfo.TargetTypeFullDisplay}> {mappingInfo.ConvertToMethodName}(this IQueryable<{mappingInfo.SourceTypeFullDisplay}> source)");
            codeBuilder.BeginSegment();
            codeBuilder.AppendCodeLines($"return source?.Select(p => new {mappingInfo.TargetTypeFullDisplay}");
            codeBuilder.BeginSegment();
            AppendPropertyAssign("p", null, ",", context);
            codeBuilder.EndSegment("}).RebuildWithIncludeForEfCore();");
            codeBuilder.EndSegment();
        }
    }
}
