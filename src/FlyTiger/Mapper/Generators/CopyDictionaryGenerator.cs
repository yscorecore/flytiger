using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FlyTiger.Mapper.Generators
{
    internal class CopyDictionaryGenerator : BaseGenerator
    {
        public override void AppendFunctions(MapperContext context)
        {
            var mappingInfo = context.MappingInfo;
            var codeBuilder = context.CodeBuilder;
            var fromType = mappingInfo.SourceType;
            var toType = mappingInfo.TargetType;
            var toTypeDisplay = mappingInfo.TargetTypeFullDisplay;
            var fromTypeDisplay = mappingInfo.SourceTypeFullDisplay;

            AddCopyToMethodForDictionary();
            void AddCopyToMethodForDictionary()
            {
                if (!mappingInfo.MapUpdate || !mappingInfo.MapConvert) return;
                if (toType.IsValueType)
                {
                    this.ReportTargetIsValueTypeCanNotCopy(context);
                    return;
                }

                codeBuilder.AppendCodeLines(
                    $"private static void {mappingInfo.ConvertToMethodName}<T>(this IDictionary<T, {fromTypeDisplay}> source, IDictionary<T, {toTypeDisplay}> target, Action<object> onRemoveItem = null, Action<object> onAddItem = null)");
                codeBuilder.BeginSegment();
                codeBuilder.AppendCodeLines($@"var sourceKeys = source.Keys;
var targetKeys = target.Keys;
// modify item
sourceKeys.Intersect(targetKeys).ForEach(key =>
{{
    {mappingInfo.ConvertToMethodName}(source[key], target[key], onRemoveItem, onAddItem);
}});
// remove item
targetKeys.Except(sourceKeys).ForEach(key =>
{{
    var item = target[key];
    target.Remove(key);
    onRemoveItem?.Invoke(item);
}});
// add item
sourceKeys.Except(targetKeys).ForEach(key =>
{{
    var item = source[key].{mappingInfo.ConvertToMethodName}();
    target.Add(key, item);
    onAddItem?.Invoke(item);
}});");

                codeBuilder.EndSegment();

            }
        }
    }
}
