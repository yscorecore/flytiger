namespace FlyTiger.Mapper.Generators
{
    internal class CopyCollectionGenerator : BaseGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var toType = mappingInfo.TargetType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;
            AddCopyToMethodForCollection();

            void AddCopyToMethodForCollection()
            {
                if (!mappingInfo.MapUpdate || !mappingInfo.MapConvert) return;
                if (toType.IsValueType)
                {
                    this.ReportTargetIsValueTypeCanNotCopy(context);
                    return;
                }

                codeBuilder.AppendCodeLines(
                    $"private static void {mappingInfo.ConvertToMethodName}<T>(this IEnumerable<{fromTypeDisplay}> source, ICollection<{toTypeDisplay}> target, CollectionUpdateMode updateMode, Func<{fromTypeDisplay}, T> sourceItemKeySelector, Func<{toTypeDisplay}, T> targetItemKeySelector, Action<object> onRemoveItem = null, Action<object> onAddItem = null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"if (updateMode == CollectionUpdateMode.Append)
{{
    source.Select(p => p.{mappingInfo.ConvertToMethodName}()).ForEach(p =>
    {{
        target.Add(p);
        onAddItem?.Invoke(p);
    }});
}}
else");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"var sourceKeys = source.Select(sourceItemKeySelector).ToHashSet();
var targetKeys = target.Select(targetItemKeySelector).ToHashSet();
// modify item
sourceKeys.Intersect(targetKeys).ForEach(key =>
{{
    {mappingInfo.ConvertToMethodName}(
        source.Where(p => sourceItemKeySelector(p).Equals(key)).First(),
        target.Where(p => targetItemKeySelector(p).Equals(key)).First(),
        onRemoveItem, onAddItem);
}});
// remove item
if (updateMode == CollectionUpdateMode.Update)
{{
    target.Where(p => !sourceKeys.Contains(targetItemKeySelector(p))).ToList().ForEach(p =>
    {{
        target.Remove(p);
        onRemoveItem?.Invoke(p);
    }});
}}
// add item
source.Where(p => !targetKeys.Contains(sourceItemKeySelector(p))).Select(p => p.{mappingInfo.ConvertToMethodName}()).ForEach(p =>
{{
    target.Add(p);
    onAddItem?.Invoke(p);
}});");

                codeBuilder.EndSegment();
                codeBuilder.EndSegment();
            }
        }
    }
}
