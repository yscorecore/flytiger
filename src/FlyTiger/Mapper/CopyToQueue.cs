using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{

    internal class CopyToQueue
    {
        Queue<CopyToMethodInfo> queue = new Queue<CopyToMethodInfo>();
        List<CopyToMethodInfo> all = new List<CopyToMethodInfo>();
        public CopyToQueue()
        {
        }
        public CopyToMethodInfo AddMethod(CopyToMethodType type, MapperContext context)
        {
            return this.TryAddInternal(type, context);
        }
        private CopyToMethodInfo TryAddInternal(CopyToMethodType type, MapperContext context)
        {
            var first = all.FirstOrDefault(t => t.CopyMethodType == type
                && t.SourceType.Equals(context.MappingInfo.SourceType, SymbolEqualityComparer.Default)
                && t.TargetType.Equals(context.MappingInfo.TargetType, SymbolEqualityComparer.Default));
            if (first != null)
            {
                return first;
            }
            else
            {
                var methodInfo = new CopyToMethodInfo(type, context);
                all.Add(methodInfo);
                queue.Enqueue(methodInfo);
                return methodInfo;
            }
        }
        public bool HasItem { get => queue.Count > 0; }

        public CopyToMethodInfo Dequeue()
        {
            return queue.Dequeue();
        }
    }

}
