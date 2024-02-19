using FluentAssertions;

namespace FlyTiger.IntegrationTest.Mapper
{
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, int>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, long>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, int?>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, long?>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<int, object>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<long, int>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, int>), typeof(TargetClass<long?, int>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, DateTime>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, DateTime?>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, DateTime>), typeof(TargetClass<int, object>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, SourceValue>), typeof(TargetClass<int, SourceValue>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, SourceValue>), typeof(TargetClass<int, TargetValue>), MapperType = MapperType.Update)]
    [Mapper(typeof(SourceClass<int, SourceValue>), typeof(TargetClass<int?, TargetValue>), MapperType = MapperType.Update)]
    public class CopyDictionaryPropertyTest
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

        #region Copy_Dic_Int32_Int32_To_Int32_Int32
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int32_Int32()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<int, int>
            {
                [2] = 21,
                [3] = 31,
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
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Int32_Int64
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int32_Int64()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<int, long>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, long>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, long>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Int32_Nullable_Int32
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int32_Nullable_Int32()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<int, int?>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, int?>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, int?>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Int32_Nullable_Int64
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int32_Nullable_Int64()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<int, long?>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, long?>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, long?>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Int32_Object
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int32_Object()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<int, object>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, object>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Int64_Int32
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Int64_Int32()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<long, int>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<long, int>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<long, int>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_Int32_To_Nullable_Int64_Int32
        [Fact]
        public void Should_Copy_Dic_Int32_Int32_To_Nullable_Int64_Int32()
        {
            var sourceDic = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
            };
            var targetDic = new Dictionary<long?, int>
            {
                [2] = 21,
                [3] = 31,
            };
            var sourceClass = new SourceClass<int, int>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<long?, int>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<long?, int>
                {
                    [1] = 1,
                    [2] = 2,
                });
        }
        #endregion

        #region Copy_Dic_Int32_DateTime_To_Int32_DateTime
        [Fact]
        public void Should_Copy_Dic_Int32_DateTime_To_Int32_DateTime()
        {
            var sourceDic = new Dictionary<int, DateTime>
            {
                [1] = new DateTime(2024, 2, 19)
            };
            var targetDic = new Dictionary<int, DateTime>
            {
                [2] = new DateTime(2024, 1, 1)
            };
            var sourceClass = new SourceClass<int, DateTime>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, DateTime>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, DateTime>
                {
                    [1] = new DateTime(2024, 2, 19)
                });
        }
        #endregion

        #region Copy_Dic_Int32_DateTime_To_Int32_Nullable_DateTime
        [Fact]
        public void Should_Copy_Dic_Int32_DateTime_To_Int32_Nullable_DateTime()
        {
            var sourceDic = new Dictionary<int, DateTime>
            {
                [1] = new DateTime(2024, 2, 19)
            };
            var targetDic = new Dictionary<int, DateTime?>
            {
                [2] = new DateTime(2024, 1, 1)
            };
            var sourceClass = new SourceClass<int, DateTime>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, DateTime?>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, DateTime?>
                {
                    [1] = new DateTime(2024, 2, 19)
                });
        }
        #endregion

        #region Copy_Dic_Int32_DateTime_To_Int32_Object
        [Fact]
        public void Should_Copy_Dic_Int32_DateTime_To_Int32_Object()
        {
            var sourceDic = new Dictionary<int, DateTime>
            {
                [1] = new DateTime(2024, 2, 19),
                [2] = new DateTime(2024, 2, 20),
            };
            var targetDic = new Dictionary<int, object>
            {
                [2] = new DateTime(2024, 1, 1),
                [3] = new DateTime(2024, 1, 2)
            };
            var sourceClass = new SourceClass<int, DateTime>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, object>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = new DateTime(2024, 2, 19),
                    [2] = new DateTime(2024, 2, 20),
                });
        }
        #endregion

        #region Copy_Dic_Int32_SourceValue_To_Int32_SourceValue
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_SourceValue()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = new SourceValue { Value = "22" },
            };
            var targetDic = new Dictionary<int, SourceValue>
            {
                [2] = new SourceValue { Value = "222" },
                [3] = new SourceValue { Value = "333" }
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, SourceValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = new SourceValue { Value = "11" },
                    [2] = new SourceValue { Value = "22" },
                });
        }
        #endregion

        #region Copy_Dic_Int32_SourceValue_To_Int32_TargetValue
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_TargetValue()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = new SourceValue { Value = "22" },
            };
            var targetDic = new Dictionary<int, TargetValue>
            {
                [2] = new TargetValue { Value = "222" },
                [3] = new TargetValue { Value = "333" }
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, TargetValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = new TargetValue { Value = "11" },
                    [2] = new TargetValue { Value = "22" },
                });
        }
        #endregion

        #region Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_SourceIsNull
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_SourceIsNull()
        {
            var targetDic = new Dictionary<int, TargetValue>
            {
                [2] = new TargetValue { Value = "222" },
                [3] = new TargetValue { Value = "333" }
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = null
            };
            var targetClass = new TargetClass<int, TargetValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeNull();
        }
        #endregion

        #region Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_TargetIsNull
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_TargetIsNull()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = new SourceValue { Value = "22" },
            };

            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, TargetValue>
            {
                Dic = null
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeEquivalentTo(new Dictionary<int, object>
            {
                [1] = new TargetValue { Value = "11" },
                [2] = new TargetValue { Value = "22" },
            });
        }
        #endregion

        #region Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_SourceContainsNull
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_SourceContainsNull()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = null,
                [4] = null,
            };
            var targetDic = new Dictionary<int, TargetValue>
            {
                [2] = new TargetValue { Value = "222" },
                [3] = new TargetValue { Value = "333" }
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, TargetValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = new TargetValue { Value = "11" },
                    [2] = null,
                    [4] = null,
                });
        }
        #endregion


        #region Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_TargetContainsNull
        [Fact]
        public void Should_Copy_Dic_Int32_SourceValue_To_Int32_TargetValue_When_TargetContainsNull()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = new SourceValue { Value = "22" },
                [4] = null,
            };
            var targetDic = new Dictionary<int, TargetValue>
            {
                [2] = null,
                [3] = new TargetValue { Value = "333" },
                [4] = null,
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int, TargetValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int, object>
                {
                    [1] = new TargetValue { Value = "11" },
                    [2] = new TargetValue { Value = "22" },
                    [4] = null,
                });
        }
        #endregion

        #region Copy_Dic_Nullable_Int32_SourceValue_To_Nullable_Int32_TargetValue
        [Fact]
        public void Should_Copy_Dic_Nullable_Int32_SourceValue_To_Nullable_Int32_TargetValue()
        {
            var sourceDic = new Dictionary<int, SourceValue>
            {
                [1] = new SourceValue { Value = "11" },
                [2] = new SourceValue { Value = "22" },
            };
            var targetDic = new Dictionary<int?, TargetValue>
            {
                [2] = new TargetValue { Value = "222" },
                [3] = new TargetValue { Value = "333" }
            };
            var sourceClass = new SourceClass<int, SourceValue>
            {
                Dic = sourceDic
            };
            var targetClass = new TargetClass<int?, TargetValue>
            {
                Dic = targetDic
            };
            sourceClass.To(targetClass);
            targetClass.Dic.Should().BeSameAs(targetDic)
                .And.BeEquivalentTo(new Dictionary<int?, object>
                {

                    [1] = new TargetValue { Value = "11" },
                    [2] = new TargetValue { Value = "22" },
                });
        }
        #endregion
    }
}
