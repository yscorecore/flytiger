using FluentAssertions;

namespace FlyTiger.IntegrationTest.Mapper
{
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, int>))]
    //[Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, int?>))]
    //[Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, long>))]
    //[Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, long>))]
    //[Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, object>))]
    //[Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, DateTime>))]
    //[Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, DateTime?>))]
    //[Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, object>))]
    //[Mapper(typeof(SourceClass<int, SourceValue>), typeof(TargetClass<int, SourceValue>))]
    //[Mapper(typeof(SourceClass<int, SourceValue>), typeof(TargetClass<int, TargetValue>))]
    public class CopyDictionaryTest
    {
        internal class SourceClass<T, V>
        {
            public Dictionary<T, V> Dic { get; set; }
        }
        internal class TargetClass<T, V>
        {
            public Dictionary<T, V> Dic { get; set; }
        }
        internal record class SourceValue
        {
            public string Value { get; set; }
        }
        internal record class TargetValue
        {
            public string Value { get; set; }
        }
        //[Fact]
        public void ShouldCopy_Dic_Int32_Int32_To_Dic_Int32_Int32()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1
            };
            var targetDic = new Dictionary<int, int>
            {
                [2] = 2
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, int>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, int>
                {
                    [1] = 1
                });

        }
    }
}
