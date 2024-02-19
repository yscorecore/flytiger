using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
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
            this.TryAddInternal(new CopyToMethodInfo(false,false,mapperContext));

        }
        public void AddCollectionCopyMethod(MapperContext mapperContext)
        {
            this.TryAddInternal(new CopyToMethodInfo(false, true,mapperContext));
        }
        public void AddDictionaryCopyMethod(MapperContext mapperContext)
        {
            this.TryAddInternal(new CopyToMethodInfo(true, true, mapperContext));
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
