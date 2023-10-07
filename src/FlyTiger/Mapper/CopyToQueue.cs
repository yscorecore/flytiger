using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger
{

    internal class CopyToQueue
    {
        Queue<CopyToMethodInfo> queue = new Queue<CopyToMethodInfo>();
        List<CopyToMethodInfo> all = new List<CopyToMethodInfo>();
        public CopyToQueue(MapperContext mapperContext)
        {
            this.AddObjectCopyMethod(mapperContext);
        }
        public void AddObjectCopyMethod(MapperContext mapperContext)
        {
            this.TryAddInternal(new CopyToMethodInfo
            {
                IsCollection = false,
                Context = mapperContext
            });

        }
        public void AddCollectionCopyMethod(MapperContext mapperContext)
        {
            this.TryAddInternal(new CopyToMethodInfo
            {
                Context = mapperContext,
                IsCollection = true,
            });
        }
        private void TryAddInternal(CopyToMethodInfo methodInfo)
        {
            if (all.Any(p => p.Equals(methodInfo)))
            {
                return;
            }
            else
            {
                all.Add(methodInfo);
                queue.Enqueue(methodInfo);
            }
        }
        public bool HasItem { get => queue.Count > 0; }

        public CopyToMethodInfo Dequeue()
        {
            return queue.Dequeue();
        }
    }

}
