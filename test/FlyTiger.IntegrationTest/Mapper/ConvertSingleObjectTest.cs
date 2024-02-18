using System.Collections.Immutable;
using FluentAssertions;

namespace FlyTiger.IntegrationTest.Mapper
{

    [Mapper(typeof(SourceUser_ArrayToIList), typeof(TargetUser_ArrayToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToICollection), typeof(TargetUser_ArrayToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToIEnumerable), typeof(TargetUser_ArrayToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToList), typeof(TargetUser_ArrayToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToIQueryable), typeof(TargetUser_ArrayToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToArray), typeof(TargetUser_ArrayToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToImmutableArray), typeof(TargetUser_ArrayToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToImmutableList), typeof(TargetUser_ArrayToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ArrayToIImmutableList), typeof(TargetUser_ArrayToIImmutableList), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_ListToIList), typeof(TargetUser_ListToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToICollection), typeof(TargetUser_ListToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToIEnumerable), typeof(TargetUser_ListToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToList), typeof(TargetUser_ListToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToIQueryable), typeof(TargetUser_ListToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToArray), typeof(TargetUser_ListToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToImmutableArray), typeof(TargetUser_ListToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToImmutableList), typeof(TargetUser_ListToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ListToIImmutableList), typeof(TargetUser_ListToIImmutableList), MapperType = MapperType.Convert)]


    [Mapper(typeof(SourceUser_IListToIList), typeof(TargetUser_IListToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToICollection), typeof(TargetUser_IListToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToIEnumerable), typeof(TargetUser_IListToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToList), typeof(TargetUser_IListToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToIQueryable), typeof(TargetUser_IListToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToArray), typeof(TargetUser_IListToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToImmutableArray), typeof(TargetUser_IListToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToImmutableList), typeof(TargetUser_IListToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IListToIImmutableList), typeof(TargetUser_IListToIImmutableList), MapperType = MapperType.Convert)]


    [Mapper(typeof(SourceUser_ICollectionToIList), typeof(TargetUser_ICollectionToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToICollection), typeof(TargetUser_ICollectionToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToIEnumerable), typeof(TargetUser_ICollectionToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToList), typeof(TargetUser_ICollectionToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToIQueryable), typeof(TargetUser_ICollectionToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToArray), typeof(TargetUser_ICollectionToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToImmutableArray), typeof(TargetUser_ICollectionToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToImmutableList), typeof(TargetUser_ICollectionToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ICollectionToIImmutableList), typeof(TargetUser_ICollectionToIImmutableList), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_IEnumerableToIList), typeof(TargetUser_IEnumerableToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToICollection), typeof(TargetUser_IEnumerableToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToIEnumerable), typeof(TargetUser_IEnumerableToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToList), typeof(TargetUser_IEnumerableToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToIQueryable), typeof(TargetUser_IEnumerableToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToArray), typeof(TargetUser_IEnumerableToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToImmutableArray), typeof(TargetUser_IEnumerableToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToImmutableList), typeof(TargetUser_IEnumerableToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IEnumerableToIImmutableList), typeof(TargetUser_IEnumerableToIImmutableList), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_IQueryableToIList), typeof(TargetUser_IQueryableToIList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToICollection), typeof(TargetUser_IQueryableToICollection), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToIEnumerable), typeof(TargetUser_IQueryableToIEnumerable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToList), typeof(TargetUser_IQueryableToList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToIQueryable), typeof(TargetUser_IQueryableToIQueryable), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToArray), typeof(TargetUser_IQueryableToArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToImmutableArray), typeof(TargetUser_IQueryableToImmutableArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToImmutableList), typeof(TargetUser_IQueryableToImmutableList), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_IQueryableToIImmutableList), typeof(TargetUser_IQueryableToIImmutableList), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_StructToClass), typeof(TargetUser_StructToClass), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ClassToStruct), typeof(TargetUser_ClassToStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_StructToStruct), typeof(TargetUser_StructToStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ClassToClass), typeof(TargetUser_ClassToClass), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_SubStructToClass), typeof(TargetUser_SubStructToClass), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubNullableStructToClass), typeof(TargetUser_SubNullableStructToClass), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubClassToStruct), typeof(TargetUser_SubClassToStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubClassToNullableStruct), typeof(TargetUser_SubClassToNullableStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubStructToStruct), typeof(TargetUser_SubStructToStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubNullableStructToStruct), typeof(TargetUser_SubNullableStructToStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubStructToNullableStruct), typeof(TargetUser_SubStructToNullableStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubNullableStructToNullableStruct), typeof(TargetUser_SubNullableStructToNullableStruct), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_SubClassToClass), typeof(TargetUser_SubClassToClass), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_StructArrayToClassArray), typeof(TargetUser_StructArrayToClassArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_StructArrayToStructArray), typeof(TargetUser_StructArrayToStructArray), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ClassArrayToStructArray), typeof(TargetUser_ClassArrayToStructArray), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceClass<int>), typeof(TargetClass<int>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int>), typeof(TargetClass<long>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int>), typeof(TargetClass<int?>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int>), typeof(TargetClass<long?>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int>), typeof(TargetClass<object>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int[]>), typeof(TargetClass<int[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<string[]>), typeof(TargetClass<string[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<string[]>), typeof(TargetClass<List<string>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int[]>), typeof(TargetClass<long[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int[]>), typeof(TargetClass<int?[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<int[]>), typeof(TargetClass<IList<long?>>), MapperType = MapperType.Convert)]


    [Mapper(typeof(SourceClass<DateTime>), typeof(TargetClass<DateTime>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<DateTime>), typeof(TargetClass<DateTime?>), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceClass<Value_TheSameSubClass>), typeof(TargetClass<Value_TheSameSubClass>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Value_ClassToBaseClass>), typeof(TargetClass<ValueParent_ClassToBaseClass>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Value_ClassToInterface>), typeof(TargetClass<IValue_ClassToInterface>), MapperType = MapperType.Convert)]


    [Mapper(typeof(SourceClass<Value_TheSameClassArray[]>), typeof(TargetClass<Value_TheSameClassArray[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Value_TheSameStructArray[]>), typeof(TargetClass<Value_TheSameStructArray[]>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Value_TheSameStructArrayToIList[]>), typeof(TargetClass<IList<Value_TheSameStructArrayToIList>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Value_TheSameStructArrayToNullableList[]>), typeof(TargetClass<List<Value_TheSameStructArrayToNullableList?>>), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceClass<Dictionary<string, string>>), typeof(TargetClass<Dictionary<string, string>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<Dictionary<int, int>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<IDictionary<int, int>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<IDictionary<int, int>>), typeof(TargetClass<IDictionary<int, int>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<IDictionary<int, int>>), typeof(TargetClass<Dictionary<int, int>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<Dictionary<int, int?>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<Dictionary<int, long>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<Dictionary<int?, int>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, int>>), typeof(TargetClass<Dictionary<int?, int?>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, DateTime>>), typeof(TargetClass<Dictionary<int, DateTime?>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Object2>>), typeof(TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Struct>>), typeof(TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct>>), typeof(TargetClass<Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct?>>), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceClass<Dictionary<SourceRecord_DictionaryObjectInt32ToDictionaryObject2Int64, int>>), typeof(TargetClass<Dictionary<TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64, long>>), MapperType = MapperType.Convert)]


    [Mapper(typeof(SourceUser_ClassToRecord), typeof(TargetUser_ClassToRecord), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_RecordToClass), typeof(TargetUser_RecordToClass), MapperType = MapperType.Convert)]

    [Mapper(typeof(SourceUser_ClassToInitOnlyRecord), typeof(TargetUser_ClassToInitOnlyRecord), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ClassToInitOnlyClass), typeof(TargetUser_ClassToInitOnlyClass), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_WithPostHandler), typeof(TargetUser_WithPostHandler), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_WithCustomMappings), typeof(TargetUser_WithCustomMappings), CustomMappings = new[]
    {
        "FullName=$.FirstName + $.LastName"
    })]
    [Mapper(typeof(SourceUser_WithIgnoreProperties), typeof(TargetUser_WithIgnoreProperties), IgnoreProperties = new[]
    {
        nameof(TargetUser_WithIgnoreProperties.LastName)
    })]
    public class ConvertSingleObjectTest
    {
        #region ArrayTo
        #region ArrayToIList
        [Fact]
        public void ShouldConvertArrayToIList()
        {
            var user = new SourceUser_ArrayToIList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToIList[]
                {
                    new SourceAddress_ArrayToIList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToIList>().Should().BeEquivalentTo(new TargetUser_ArrayToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ArrayToIList[] {
                    new TargetAddress_ArrayToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ArrayToIList
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToIList[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_ArrayToIList> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToICollection
        [Fact]
        public void ShouldConvertArrayToICollection()
        {
            var user = new SourceUser_ArrayToICollection
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToICollection[]
                {
                    new SourceAddress_ArrayToICollection{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToICollection>().Should().BeEquivalentTo(new TargetUser_ArrayToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ArrayToICollection[] {
                    new TargetAddress_ArrayToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ArrayToICollection
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToICollection[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_ArrayToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToIEnumerable
        [Fact]
        public void ShouldConvertArrayToIEnumerable()
        {
            var user = new SourceUser_ArrayToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToIEnumerable[]
                {
                    new SourceAddress_ArrayToIEnumerable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToIEnumerable>().Should().BeEquivalentTo(new TargetUser_ArrayToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ArrayToIEnumerable[] {
                    new TargetAddress_ArrayToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ArrayToIEnumerable
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToIEnumerable[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_ArrayToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToList
        [Fact]
        public void ShouldConvertArrayToList()
        {
            var user = new SourceUser_ArrayToList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToList[]
                {
                    new SourceAddress_ArrayToList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToList>().Should().BeEquivalentTo(new TargetUser_ArrayToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_ArrayToList> {
                    new TargetAddress_ArrayToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ArrayToList
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToList[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToList
        {
            public string Name { get; set; }
            public List<TargetAddress_ArrayToList> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToList
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToIQueryable
        [Fact]
        public void ShouldConvertArrayToIQueryable()
        {
            var user = new SourceUser_ArrayToIQueryable
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToIQueryable[]
                {
                    new SourceAddress_ArrayToIQueryable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToIQueryable>().Should().BeEquivalentTo(new TargetUser_ArrayToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ArrayToIQueryable[] {
                    new TargetAddress_ArrayToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_ArrayToIQueryable
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToIQueryable[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_ArrayToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToArray
        [Fact]
        public void ShouldConvertArrayToArray()
        {
            var user = new SourceUser_ArrayToArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToArray[]
                {
                    new SourceAddress_ArrayToArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToArray>().Should().BeEquivalentTo(new TargetUser_ArrayToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ArrayToArray[] {
                    new TargetAddress_ArrayToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ArrayToArray
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToArray[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToArray
        {
            public string Name { get; set; }
            public TargetAddress_ArrayToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToImmutableArray
        [Fact]
        public void ShouldConvertArrayToImmutableArray()
        {
            var user = new SourceUser_ArrayToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToImmutableArray[]
                {
                    new SourceAddress_ArrayToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToImmutableArray>().Should().BeEquivalentTo(new TargetUser_ArrayToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_ArrayToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ArrayToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_ArrayToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToImmutableList
        [Fact]
        public void ShouldConvertArrayToImmutableList()
        {
            var user = new SourceUser_ArrayToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToImmutableList[]
                {
                    new SourceAddress_ArrayToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToImmutableList>().Should().BeEquivalentTo(new TargetUser_ArrayToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ArrayToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ArrayToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_ArrayToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region ArrayToIImmutableList
        [Fact]
        public void ShouldConvertArrayToIImmutableList()
        {
            var user = new SourceUser_ArrayToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ArrayToIImmutableList[]
                {
                    new SourceAddress_ArrayToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ArrayToIImmutableList>().Should().BeEquivalentTo(new TargetUser_ArrayToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ArrayToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ArrayToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ArrayToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ArrayToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ArrayToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_ArrayToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ArrayToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion
        #endregion

        #region ListTo
        #region ListToIList
        [Fact]
        public void ShouldConvertListToIList()
        {
            var user = new SourceUser_ListToIList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToIList>
                {
                    new SourceAddress_ListToIList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToIList>().Should().BeEquivalentTo(new TargetUser_ListToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ListToIList[] {
                    new TargetAddress_ListToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ListToIList
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToIList> Addresses { get; set; }
        }
        internal class SourceAddress_ListToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_ListToIList> Addresses { get; set; }
        }
        internal class TargetAddress_ListToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToICollection
        [Fact]
        public void ShouldConvertListToICollection()
        {
            var user = new SourceUser_ListToICollection
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToICollection>
                {
                    new SourceAddress_ListToICollection{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToICollection>().Should().BeEquivalentTo(new TargetUser_ListToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ListToICollection[] {
                    new TargetAddress_ListToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ListToICollection
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToICollection> Addresses { get; set; }
        }
        internal class SourceAddress_ListToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_ListToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_ListToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToIEnumerable
        [Fact]
        public void ShouldConvertListToIEnumerable()
        {
            var user = new SourceUser_ListToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToIEnumerable>
                {
                    new SourceAddress_ListToIEnumerable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToIEnumerable>().Should().BeEquivalentTo(new TargetUser_ListToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ListToIEnumerable[] {
                    new TargetAddress_ListToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ListToIEnumerable
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToIEnumerable> Addresses { get; set; }
        }
        internal class SourceAddress_ListToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_ListToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_ListToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToList
        [Fact]
        public void ShouldConvertListToList()
        {
            var user = new SourceUser_ListToList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToList>
                {
                    new SourceAddress_ListToList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToList>().Should().BeEquivalentTo(new TargetUser_ListToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_ListToList> {
                    new TargetAddress_ListToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ListToList
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToList> Addresses { get; set; }
        }
        internal class SourceAddress_ListToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToList
        {
            public string Name { get; set; }
            public List<TargetAddress_ListToList> Addresses { get; set; }
        }
        internal class TargetAddress_ListToList
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToIQueryable
        [Fact]
        public void ShouldConvertListToIQueryable()
        {
            var user = new SourceUser_ListToIQueryable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToIQueryable>
                {
                    new SourceAddress_ListToIQueryable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToIQueryable>().Should().BeEquivalentTo(new TargetUser_ListToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ListToIQueryable[] {
                    new TargetAddress_ListToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_ListToIQueryable
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToIQueryable> Addresses { get; set; }
        }
        internal class SourceAddress_ListToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_ListToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_ListToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToArray
        [Fact]
        public void ShouldConvertListToArray()
        {
            var user = new SourceUser_ListToArray
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ListToArray>
                {
                    new SourceAddress_ListToArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToArray>().Should().BeEquivalentTo(new TargetUser_ListToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ListToArray[] {
                    new TargetAddress_ListToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ListToArray
        {
            public string Name { get; set; }
            public List<SourceAddress_ListToArray> Addresses { get; set; }
        }
        internal class SourceAddress_ListToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToArray
        {
            public string Name { get; set; }
            public TargetAddress_ListToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_ListToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToImmutableArray
        [Fact]
        public void ShouldConvertListToImmutableArray()
        {
            var user = new SourceUser_ListToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ListToImmutableArray[]
                {
                    new SourceAddress_ListToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToImmutableArray>().Should().BeEquivalentTo(new TargetUser_ListToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_ListToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ListToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_ListToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_ListToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_ListToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_ListToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToImmutableList
        [Fact]
        public void ShouldConvertListToImmutableList()
        {
            var user = new SourceUser_ListToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ListToImmutableList[]
                {
                    new SourceAddress_ListToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToImmutableList>().Should().BeEquivalentTo(new TargetUser_ListToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ListToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ListToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ListToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ListToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_ListToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ListToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region ListToIImmutableList
        [Fact]
        public void ShouldConvertListToIImmutableList()
        {
            var user = new SourceUser_ListToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ListToIImmutableList[]
                {
                    new SourceAddress_ListToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ListToIImmutableList>().Should().BeEquivalentTo(new TargetUser_ListToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ListToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ListToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ListToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ListToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ListToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_ListToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ListToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion
        #endregion

        #region IListTo
        #region IListToIList
        [Fact]
        public void ShouldConvertIListToIList()
        {
            var user = new SourceUser_IListToIList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToIList>
                {
                    new SourceAddress_IListToIList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToIList>().Should().BeEquivalentTo(new TargetUser_IListToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IListToIList[] {
                    new TargetAddress_IListToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IListToIList
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToIList> Addresses { get; set; }
        }
        internal class SourceAddress_IListToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_IListToIList> Addresses { get; set; }
        }
        internal class TargetAddress_IListToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToICollection
        [Fact]
        public void ShouldConvertIListToICollection()
        {
            var user = new SourceUser_IListToICollection
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToICollection>
                {
                    new SourceAddress_IListToICollection{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToICollection>().Should().BeEquivalentTo(new TargetUser_IListToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IListToICollection[] {
                    new TargetAddress_IListToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IListToICollection
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToICollection> Addresses { get; set; }
        }
        internal class SourceAddress_IListToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_IListToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_IListToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToIEnumerable
        [Fact]
        public void ShouldConvertIListToIEnumerable()
        {
            var user = new SourceUser_IListToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToIEnumerable>
                {
                    new SourceAddress_IListToIEnumerable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToIEnumerable>().Should().BeEquivalentTo(new TargetUser_IListToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IListToIEnumerable[] {
                    new TargetAddress_IListToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IListToIEnumerable
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToIEnumerable> Addresses { get; set; }
        }
        internal class SourceAddress_IListToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_IListToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_IListToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToList
        [Fact]
        public void ShouldConvertIListToList()
        {
            var user = new SourceUser_IListToList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToList>
                {
                    new SourceAddress_IListToList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToList>().Should().BeEquivalentTo(new TargetUser_IListToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_IListToList> {
                    new TargetAddress_IListToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IListToList
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToList> Addresses { get; set; }
        }
        internal class SourceAddress_IListToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToList
        {
            public string Name { get; set; }
            public List<TargetAddress_IListToList> Addresses { get; set; }
        }
        internal class TargetAddress_IListToList
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToIQueryable
        [Fact]
        public void ShouldConvertIListToIQueryable()
        {
            var user = new SourceUser_IListToIQueryable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToIQueryable>
                {
                    new SourceAddress_IListToIQueryable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToIQueryable>().Should().BeEquivalentTo(new TargetUser_IListToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IListToIQueryable[] {
                    new TargetAddress_IListToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_IListToIQueryable
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToIQueryable> Addresses { get; set; }
        }
        internal class SourceAddress_IListToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_IListToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_IListToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToArray
        [Fact]
        public void ShouldConvertIListToArray()
        {
            var user = new SourceUser_IListToArray
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IListToArray>
                {
                    new SourceAddress_IListToArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToArray>().Should().BeEquivalentTo(new TargetUser_IListToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IListToArray[] {
                    new TargetAddress_IListToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IListToArray
        {
            public string Name { get; set; }
            public IList<SourceAddress_IListToArray> Addresses { get; set; }
        }
        internal class SourceAddress_IListToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToArray
        {
            public string Name { get; set; }
            public TargetAddress_IListToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_IListToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToImmutableArray
        [Fact]
        public void ShouldConvertIListToImmutableArray()
        {
            var user = new SourceUser_IListToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IListToImmutableArray[]
                {
                    new SourceAddress_IListToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToImmutableArray>().Should().BeEquivalentTo(new TargetUser_IListToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_IListToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IListToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_IListToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_IListToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_IListToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_IListToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToImmutableList
        [Fact]
        public void ShouldConvertIListToImmutableList()
        {
            var user = new SourceUser_IListToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IListToImmutableList[]
                {
                    new SourceAddress_IListToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToImmutableList>().Should().BeEquivalentTo(new TargetUser_IListToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IListToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IListToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IListToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IListToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_IListToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IListToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region IListToIImmutableList
        [Fact]
        public void ShouldConvertIListToIImmutableList()
        {
            var user = new SourceUser_IListToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IListToIImmutableList[]
                {
                    new SourceAddress_IListToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IListToIImmutableList>().Should().BeEquivalentTo(new TargetUser_IListToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IListToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IListToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IListToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IListToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IListToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_IListToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IListToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #endregion

        #region ICollectionTo
        #region ICollectionToIList
        [Fact]
        public void ShouldConvertICollectionToIList()
        {
            var user = new SourceUser_ICollectionToIList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToIList>
                {
                    new SourceAddress_ICollectionToIList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToIList>().Should().BeEquivalentTo(new TargetUser_ICollectionToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ICollectionToIList[] {
                    new TargetAddress_ICollectionToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ICollectionToIList
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToIList> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_ICollectionToIList> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToICollection
        [Fact]
        public void ShouldConvertICollectionToICollection()
        {
            var user = new SourceUser_ICollectionToICollection
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToICollection>
                {
                    new SourceAddress_ICollectionToICollection{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToICollection>().Should().BeEquivalentTo(new TargetUser_ICollectionToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ICollectionToICollection[] {
                    new TargetAddress_ICollectionToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ICollectionToICollection
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToICollection> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_ICollectionToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToIEnumerable
        [Fact]
        public void ShouldConvertICollectionToIEnumerable()
        {
            var user = new SourceUser_ICollectionToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToIEnumerable>
                {
                    new SourceAddress_ICollectionToIEnumerable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToIEnumerable>().Should().BeEquivalentTo(new TargetUser_ICollectionToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ICollectionToIEnumerable[] {
                    new TargetAddress_ICollectionToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ICollectionToIEnumerable
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToIEnumerable> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_ICollectionToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToList
        [Fact]
        public void ShouldConvertICollectionToList()
        {
            var user = new SourceUser_ICollectionToList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToList>
                {
                    new SourceAddress_ICollectionToList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToList>().Should().BeEquivalentTo(new TargetUser_ICollectionToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_ICollectionToList> {
                    new TargetAddress_ICollectionToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ICollectionToList
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToList> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToList
        {
            public string Name { get; set; }
            public List<TargetAddress_ICollectionToList> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToList
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToIQueryable
        [Fact]
        public void ShouldConvertICollectionToIQueryable()
        {
            var user = new SourceUser_ICollectionToIQueryable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToIQueryable>
                {
                    new SourceAddress_ICollectionToIQueryable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToIQueryable>().Should().BeEquivalentTo(new TargetUser_ICollectionToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ICollectionToIQueryable[] {
                    new TargetAddress_ICollectionToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_ICollectionToIQueryable
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToIQueryable> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_ICollectionToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToArray
        [Fact]
        public void ShouldConvertICollectionToArray()
        {
            var user = new SourceUser_ICollectionToArray
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_ICollectionToArray>
                {
                    new SourceAddress_ICollectionToArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToArray>().Should().BeEquivalentTo(new TargetUser_ICollectionToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ICollectionToArray[] {
                    new TargetAddress_ICollectionToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ICollectionToArray
        {
            public string Name { get; set; }
            public ICollection<SourceAddress_ICollectionToArray> Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToArray
        {
            public string Name { get; set; }
            public TargetAddress_ICollectionToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToImmutableArray
        [Fact]
        public void ShouldConvertICollectionToImmutableArray()
        {
            var user = new SourceUser_ICollectionToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ICollectionToImmutableArray[]
                {
                    new SourceAddress_ICollectionToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToImmutableArray>().Should().BeEquivalentTo(new TargetUser_ICollectionToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_ICollectionToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ICollectionToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_ICollectionToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_ICollectionToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToImmutableList
        [Fact]
        public void ShouldConvertICollectionToImmutableList()
        {
            var user = new SourceUser_ICollectionToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ICollectionToImmutableList[]
                {
                    new SourceAddress_ICollectionToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToImmutableList>().Should().BeEquivalentTo(new TargetUser_ICollectionToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ICollectionToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ICollectionToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ICollectionToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_ICollectionToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region ICollectionToIImmutableList
        [Fact]
        public void ShouldConvertICollectionToIImmutableList()
        {
            var user = new SourceUser_ICollectionToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ICollectionToIImmutableList[]
                {
                    new SourceAddress_ICollectionToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ICollectionToIImmutableList>().Should().BeEquivalentTo(new TargetUser_ICollectionToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_ICollectionToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_ICollectionToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_ICollectionToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_ICollectionToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_ICollectionToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_ICollectionToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_ICollectionToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion
        #endregion

        #region IEnumerableTo
        #region IEnumerableToIList
        [Fact]
        public void ShouldConvertIEnumerableToIList()
        {
            var user = new SourceUser_IEnumerableToIList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToIList>
                {
                    new SourceAddress_IEnumerableToIList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToIList>().Should().BeEquivalentTo(new TargetUser_IEnumerableToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IEnumerableToIList[] {
                    new TargetAddress_IEnumerableToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IEnumerableToIList
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToIList> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_IEnumerableToIList> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToICollection
        [Fact]
        public void ShouldConvertIEnumerableToICollection()
        {
            var user = new SourceUser_IEnumerableToICollection
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToICollection>
                {
                    new SourceAddress_IEnumerableToICollection{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToICollection>().Should().BeEquivalentTo(new TargetUser_IEnumerableToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IEnumerableToICollection[] {
                    new TargetAddress_IEnumerableToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IEnumerableToICollection
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToICollection> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_IEnumerableToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToIEnumerable
        [Fact]
        public void ShouldConvertIEnumerableToIEnumerable()
        {
            var user = new SourceUser_IEnumerableToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToIEnumerable>
                {
                    new SourceAddress_IEnumerableToIEnumerable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToIEnumerable>().Should().BeEquivalentTo(new TargetUser_IEnumerableToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IEnumerableToIEnumerable[] {
                    new TargetAddress_IEnumerableToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IEnumerableToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToIEnumerable> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_IEnumerableToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToList
        [Fact]
        public void ShouldConvertIEnumerableToList()
        {
            var user = new SourceUser_IEnumerableToList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToList>
                {
                    new SourceAddress_IEnumerableToList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToList>().Should().BeEquivalentTo(new TargetUser_IEnumerableToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_IEnumerableToList> {
                    new TargetAddress_IEnumerableToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IEnumerableToList
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToList> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToList
        {
            public string Name { get; set; }
            public List<TargetAddress_IEnumerableToList> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToList
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToIQueryable
        [Fact]
        public void ShouldConvertIEnumerableToIQueryable()
        {
            var user = new SourceUser_IEnumerableToIQueryable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToIQueryable>
                {
                    new SourceAddress_IEnumerableToIQueryable{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToIQueryable>().Should().BeEquivalentTo(new TargetUser_IEnumerableToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IEnumerableToIQueryable[] {
                    new TargetAddress_IEnumerableToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_IEnumerableToIQueryable
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToIQueryable> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_IEnumerableToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToArray
        [Fact]
        public void ShouldConvertIEnumerableToArray()
        {
            var user = new SourceUser_IEnumerableToArray
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IEnumerableToArray>
                {
                    new SourceAddress_IEnumerableToArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToArray>().Should().BeEquivalentTo(new TargetUser_IEnumerableToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IEnumerableToArray[] {
                    new TargetAddress_IEnumerableToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IEnumerableToArray
        {
            public string Name { get; set; }
            public IEnumerable<SourceAddress_IEnumerableToArray> Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToArray
        {
            public string Name { get; set; }
            public TargetAddress_IEnumerableToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToImmutableArray
        [Fact]
        public void ShouldConvertIEnumerableToImmutableArray()
        {
            var user = new SourceUser_IEnumerableToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IEnumerableToImmutableArray[]
                {
                    new SourceAddress_IEnumerableToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToImmutableArray>().Should().BeEquivalentTo(new TargetUser_IEnumerableToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_IEnumerableToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IEnumerableToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_IEnumerableToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_IEnumerableToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToImmutableList
        [Fact]
        public void ShouldConvertIEnumerableToImmutableList()
        {
            var user = new SourceUser_IEnumerableToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IEnumerableToImmutableList[]
                {
                    new SourceAddress_IEnumerableToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToImmutableList>().Should().BeEquivalentTo(new TargetUser_IEnumerableToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IEnumerableToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IEnumerableToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IEnumerableToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_IEnumerableToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region IEnumerableToIImmutableList
        [Fact]
        public void ShouldConvertIEnumerableToIImmutableList()
        {
            var user = new SourceUser_IEnumerableToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IEnumerableToIImmutableList[]
                {
                    new SourceAddress_IEnumerableToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IEnumerableToIImmutableList>().Should().BeEquivalentTo(new TargetUser_IEnumerableToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IEnumerableToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IEnumerableToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IEnumerableToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IEnumerableToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IEnumerableToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_IEnumerableToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IEnumerableToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion
        #endregion

        #region IQueryableTo
        #region IQueryableToIList
        [Fact]
        public void ShouldConvertIQueryableToIList()
        {
            var user = new SourceUser_IQueryableToIList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToIList>
                {
                    new SourceAddress_IQueryableToIList{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToIList>().Should().BeEquivalentTo(new TargetUser_IQueryableToIList
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IQueryableToIList[] {
                    new TargetAddress_IQueryableToIList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IQueryableToIList
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToIList> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToIList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToIList
        {
            public string Name { get; set; }
            public IList<TargetAddress_IQueryableToIList> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToIList
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToICollection
        [Fact]
        public void ShouldConvertIQueryableToICollection()
        {
            var user = new SourceUser_IQueryableToICollection
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToICollection>
                {
                    new SourceAddress_IQueryableToICollection{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToICollection>().Should().BeEquivalentTo(new TargetUser_IQueryableToICollection
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IQueryableToICollection[] {
                    new TargetAddress_IQueryableToICollection{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IQueryableToICollection
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToICollection> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToICollection
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToICollection
        {
            public string Name { get; set; }
            public ICollection<TargetAddress_IQueryableToICollection> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToICollection
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToIEnumerable
        [Fact]
        public void ShouldConvertIQueryableToIEnumerable()
        {
            var user = new SourceUser_IQueryableToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToIEnumerable>
                {
                    new SourceAddress_IQueryableToIEnumerable{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToIEnumerable>().Should().BeEquivalentTo(new TargetUser_IQueryableToIEnumerable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IQueryableToIEnumerable[] {
                    new TargetAddress_IQueryableToIEnumerable{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IQueryableToIEnumerable
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToIEnumerable> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToIEnumerable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToIEnumerable
        {
            public string Name { get; set; }
            public IEnumerable<TargetAddress_IQueryableToIEnumerable> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToIEnumerable
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToList
        [Fact]
        public void ShouldConvertIQueryableToList()
        {
            var user = new SourceUser_IQueryableToList
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToList>
                {
                    new SourceAddress_IQueryableToList{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToList>().Should().BeEquivalentTo(new TargetUser_IQueryableToList
            {
                Name = "zhangsan",
                Addresses = new List<TargetAddress_IQueryableToList> {
                    new TargetAddress_IQueryableToList{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IQueryableToList
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToList> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToList
        {
            public string Name { get; set; }
            public List<TargetAddress_IQueryableToList> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToList
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToIQueryable
        [Fact]
        public void ShouldConvertIQueryableToIQueryable()
        {
            var user = new SourceUser_IQueryableToIQueryable
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToIQueryable>
                {
                    new SourceAddress_IQueryableToIQueryable{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToIQueryable>().Should().BeEquivalentTo(new TargetUser_IQueryableToIQueryable
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IQueryableToIQueryable[] {
                    new TargetAddress_IQueryableToIQueryable{ City ="xi'an"}
                }.AsQueryable()
            });
        }
        internal class SourceUser_IQueryableToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToIQueryable> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToIQueryable
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToIQueryable
        {
            public string Name { get; set; }
            public IQueryable<TargetAddress_IQueryableToIQueryable> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToIQueryable
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToArray
        [Fact]
        public void ShouldConvertIQueryableToArray()
        {
            var user = new SourceUser_IQueryableToArray
            {
                Name = "zhangsan",
                Addresses = new List<SourceAddress_IQueryableToArray>
                {
                    new SourceAddress_IQueryableToArray{ City = "xi'an" }
                }.AsQueryable()
            };
            user.To<TargetUser_IQueryableToArray>().Should().BeEquivalentTo(new TargetUser_IQueryableToArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_IQueryableToArray[] {
                    new TargetAddress_IQueryableToArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_IQueryableToArray
        {
            public string Name { get; set; }
            public IQueryable<SourceAddress_IQueryableToArray> Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToArray
        {
            public string Name { get; set; }
            public TargetAddress_IQueryableToArray[] Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToImmutableArray
        [Fact]
        public void ShouldConvertIQueryableToImmutableArray()
        {
            var user = new SourceUser_IQueryableToImmutableArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IQueryableToImmutableArray[]
                {
                    new SourceAddress_IQueryableToImmutableArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IQueryableToImmutableArray>().Should().BeEquivalentTo(new TargetUser_IQueryableToImmutableArray
            {
                Name = "zhangsan",
                Addresses = ImmutableArray.Create(
                    new TargetAddress_IQueryableToImmutableArray { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IQueryableToImmutableArray
        {
            public string Name { get; set; }
            public SourceAddress_IQueryableToImmutableArray[] Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToImmutableArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToImmutableArray
        {
            public string Name { get; set; }
            public ImmutableArray<TargetAddress_IQueryableToImmutableArray> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToImmutableArray
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToImmutableList
        [Fact]
        public void ShouldConvertIQueryableToImmutableList()
        {
            var user = new SourceUser_IQueryableToImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IQueryableToImmutableList[]
                {
                    new SourceAddress_IQueryableToImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IQueryableToImmutableList>().Should().BeEquivalentTo(new TargetUser_IQueryableToImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IQueryableToImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IQueryableToImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IQueryableToImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToImmutableList
        {
            public string Name { get; set; }
            public ImmutableList<TargetAddress_IQueryableToImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToImmutableList
        {
            public string City { get; set; }
        }
        #endregion

        #region IQueryableToIImmutableList
        [Fact]
        public void ShouldConvertIQueryableToIImmutableList()
        {
            var user = new SourceUser_IQueryableToIImmutableList
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_IQueryableToIImmutableList[]
                {
                    new SourceAddress_IQueryableToIImmutableList{ City = "xi'an" }
                }
            };
            user.To<TargetUser_IQueryableToIImmutableList>().Should().BeEquivalentTo(new TargetUser_IQueryableToIImmutableList
            {
                Name = "zhangsan",
                Addresses = ImmutableList.Create(
                    new TargetAddress_IQueryableToIImmutableList { City = "xi'an" }
                )
            });
        }
        internal class SourceUser_IQueryableToIImmutableList
        {
            public string Name { get; set; }
            public SourceAddress_IQueryableToIImmutableList[] Addresses { get; set; }
        }
        internal class SourceAddress_IQueryableToIImmutableList
        {
            public string City { get; set; }
        }

        internal class TargetUser_IQueryableToIImmutableList
        {
            public string Name { get; set; }
            public IImmutableList<TargetAddress_IQueryableToIImmutableList> Addresses { get; set; }
        }
        internal class TargetAddress_IQueryableToIImmutableList
        {
            public string City { get; set; }
        }
        #endregion
        #endregion

        #region StructToClass
        [Fact]
        public void ShouldConvertStructToClass()
        {
            var user = new SourceUser_StructToClass
            {
                Name = "zhangsan",
            };
            user.To<TargetUser_StructToClass>().Should().BeEquivalentTo(new TargetUser_StructToClass
            {
                Name = "zhangsan",
            });
        }

        internal struct SourceUser_StructToClass
        {
            public string Name { get; set; }
        }


        internal class TargetUser_StructToClass
        {
            public string Name { get; set; }
        }

        #endregion

        #region ClassToStruct
        [Fact]
        public void ShouldConvertClassToStruct()
        {
            var user = new SourceUser_ClassToStruct
            {
                Name = "zhangsan",
            };
            user.To<TargetUser_ClassToStruct>().Should().BeEquivalentTo(new TargetUser_ClassToStruct
            {
                Name = "zhangsan",
            });
        }

        internal class SourceUser_ClassToStruct
        {
            public string Name { get; set; }
        }


        internal struct TargetUser_ClassToStruct
        {
            public string Name { get; set; }
        }


        #endregion

        #region StructToStruct
        [Fact]
        public void ShouldConvertStructToStruct()
        {
            var user = new SourceUser_StructToStruct
            {
                Name = "zhangsan",
            };
            user.To<TargetUser_StructToStruct>().Should().BeEquivalentTo(new TargetUser_StructToStruct
            {
                Name = "zhangsan",
            });
        }

        internal struct SourceUser_StructToStruct
        {
            public string Name { get; set; }
        }


        internal struct TargetUser_StructToStruct
        {
            public string Name { get; set; }
        }


        #endregion

        #region ClassToClass
        [Fact]
        public void ShouldConvertClassToClass()
        {
            var user = new SourceUser_ClassToClass
            {
                Name = "zhangsan",
            };
            user.To<TargetUser_ClassToClass>().Should().BeEquivalentTo(new TargetUser_ClassToClass
            {
                Name = "zhangsan",
            });
        }

        internal class SourceUser_ClassToClass
        {
            public string Name { get; set; }
        }


        internal class TargetUser_ClassToClass
        {
            public string Name { get; set; }
        }


        #endregion

        #region SubStructToClass
        [Fact]
        public void ShouldConvertSubStructToClass()
        {
            var user = new SourceUser_SubStructToClass
            {
                Name = "zhangsan",
                Role = new SourceRole_SubStructToClass
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubStructToClass>().Should().BeEquivalentTo(new TargetUser_SubStructToClass
            {
                Name = "zhangsan",
                Role = new TargetRole_SubStructToClass
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubStructToClass
        {
            public string Name { get; set; }
            public SourceRole_SubStructToClass Role { get; set; }
        }
        internal struct SourceRole_SubStructToClass
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubStructToClass
        {
            public string Name { get; set; }
            public TargetRole_SubStructToClass Role { get; set; }
        }
        internal class TargetRole_SubStructToClass
        {
            public string Name { get; set; }
        }

        #endregion

        #region SubNullableStructToClass
        [Fact]
        public void ShouldConvertSubNullableStructToClass()
        {
            var user = new SourceUser_SubNullableStructToClass
            {
                Name = "zhangsan",
                Role = new SourceRole_SubNullableStructToClass
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubNullableStructToClass>().Should().BeEquivalentTo(new TargetUser_SubNullableStructToClass
            {
                Name = "zhangsan",
                Role = new TargetRole_SubNullableStructToClass
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubNullableStructToClass
        {
            public string Name { get; set; }
            public SourceRole_SubNullableStructToClass? Role { get; set; }
        }
        internal struct SourceRole_SubNullableStructToClass
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubNullableStructToClass
        {
            public string Name { get; set; }
            public TargetRole_SubNullableStructToClass Role { get; set; }
        }
        internal class TargetRole_SubNullableStructToClass
        {
            public string Name { get; set; }
        }

        #endregion

        #region SubClassToStruct
        [Fact]
        public void ShouldConvertSubClassToStruct()
        {
            var user = new SourceUser_SubClassToStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubClassToStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubClassToStruct>().Should().BeEquivalentTo(new TargetUser_SubClassToStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubClassToStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubClassToStruct
        {
            public string Name { get; set; }
            public SourceRole_SubClassToStruct Role { get; set; }
        }
        internal class SourceRole_SubClassToStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubClassToStruct
        {
            public string Name { get; set; }
            public TargetRole_SubClassToStruct Role { get; set; }
        }
        internal struct TargetRole_SubClassToStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubClassToNullableStruct
        [Fact]
        public void ShouldConvertSubClassToNullableStruct()
        {
            var user = new SourceUser_SubClassToNullableStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubClassToNullableStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubClassToNullableStruct>().Should().BeEquivalentTo(new TargetUser_SubClassToNullableStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubClassToNullableStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubClassToNullableStruct
        {
            public string Name { get; set; }
            public SourceRole_SubClassToNullableStruct Role { get; set; }
        }
        internal class SourceRole_SubClassToNullableStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubClassToNullableStruct
        {
            public string Name { get; set; }
            public TargetRole_SubClassToNullableStruct Role { get; set; }
        }
        internal struct TargetRole_SubClassToNullableStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubStructToStruct
        [Fact]
        public void ShouldConvertSubStructToStruct()
        {
            var user = new SourceUser_SubStructToStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubStructToStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubStructToStruct>().Should().BeEquivalentTo(new TargetUser_SubStructToStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubStructToStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubStructToStruct
        {
            public string Name { get; set; }
            public SourceRole_SubStructToStruct Role { get; set; }
        }
        internal struct SourceRole_SubStructToStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubStructToStruct
        {
            public string Name { get; set; }
            public TargetRole_SubStructToStruct Role { get; set; }
        }
        internal struct TargetRole_SubStructToStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubNullableStructToStruct
        [Fact]
        public void ShouldConvertSubNullableStructToStruct()
        {
            var user = new SourceUser_SubNullableStructToStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubNullableStructToStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubNullableStructToStruct>().Should().BeEquivalentTo(new TargetUser_SubNullableStructToStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubNullableStructToStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubNullableStructToStruct
        {
            public string Name { get; set; }
            public SourceRole_SubNullableStructToStruct? Role { get; set; }
        }
        internal struct SourceRole_SubNullableStructToStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubNullableStructToStruct
        {
            public string Name { get; set; }
            public TargetRole_SubNullableStructToStruct Role { get; set; }
        }
        internal struct TargetRole_SubNullableStructToStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubStructToNullableStruct
        [Fact]
        public void ShouldConvertSubStructToNullableStruct()
        {
            var user = new SourceUser_SubStructToNullableStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubStructToNullableStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubStructToNullableStruct>().Should().BeEquivalentTo(new TargetUser_SubStructToNullableStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubStructToNullableStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubStructToNullableStruct
        {
            public string Name { get; set; }
            public SourceRole_SubStructToNullableStruct Role { get; set; }
        }
        internal struct SourceRole_SubStructToNullableStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubStructToNullableStruct
        {
            public string Name { get; set; }
            public TargetRole_SubStructToNullableStruct? Role { get; set; }
        }
        internal struct TargetRole_SubStructToNullableStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubNullableStructToNullableStruct
        [Fact]
        public void ShouldConvertSubNullableStructToNullableStruct()
        {
            var user = new SourceUser_SubNullableStructToNullableStruct
            {
                Name = "zhangsan",
                Role = new SourceRole_SubNullableStructToNullableStruct
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubNullableStructToNullableStruct>().Should().BeEquivalentTo(new TargetUser_SubNullableStructToNullableStruct
            {
                Name = "zhangsan",
                Role = new TargetRole_SubNullableStructToNullableStruct
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubNullableStructToNullableStruct
        {
            public string Name { get; set; }
            public SourceRole_SubNullableStructToNullableStruct? Role { get; set; }
        }
        internal struct SourceRole_SubNullableStructToNullableStruct
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubNullableStructToNullableStruct
        {
            public string Name { get; set; }
            public TargetRole_SubNullableStructToNullableStruct? Role { get; set; }
        }
        internal struct TargetRole_SubNullableStructToNullableStruct
        {
            public string Name { get; set; }
        }
        #endregion

        #region SubClassToClass
        [Fact]
        public void ShouldConvertSubClassToClass()
        {
            var user = new SourceUser_SubClassToClass
            {
                Name = "zhangsan",
                Role = new SourceRole_SubClassToClass
                {
                    Name = "role1"
                }
            };
            user.To<TargetUser_SubClassToClass>().Should().BeEquivalentTo(new TargetUser_SubClassToClass
            {
                Name = "zhangsan",
                Role = new TargetRole_SubClassToClass
                {
                    Name = "role1"
                }
            });
        }

        internal class SourceUser_SubClassToClass
        {
            public string Name { get; set; }
            public SourceRole_SubClassToClass Role { get; set; }
        }
        internal class SourceRole_SubClassToClass
        {
            public string Name { get; set; }
        }
        internal class TargetUser_SubClassToClass
        {
            public string Name { get; set; }
            public TargetRole_SubClassToClass Role { get; set; }
        }
        internal struct TargetRole_SubClassToClass
        {
            public string Name { get; set; }
        }
        #endregion

        #region SturctArrayToClassArray
        [Fact]
        public void ShouldConvertStructArrayToClassArray()
        {
            var user = new SourceUser_StructArrayToClassArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_StructArrayToClassArray[]
                {
                    new SourceAddress_StructArrayToClassArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_StructArrayToClassArray>().Should().BeEquivalentTo(new TargetUser_StructArrayToClassArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_StructArrayToClassArray[] {
                    new TargetAddress_StructArrayToClassArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_StructArrayToClassArray
        {
            public string Name { get; set; }
            public SourceAddress_StructArrayToClassArray[] Addresses { get; set; }
        }
        internal struct SourceAddress_StructArrayToClassArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_StructArrayToClassArray
        {
            public string Name { get; set; }
            public TargetAddress_StructArrayToClassArray[] Addresses { get; set; }
        }
        internal class TargetAddress_StructArrayToClassArray
        {
            public string City { get; set; }
        }
        #endregion

        #region SturctArrayToStructArray
        [Fact]
        public void ShouldConvertStructArrayToStructArray()
        {
            var user = new SourceUser_StructArrayToStructArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_StructArrayToStructArray[]
                {
                    new SourceAddress_StructArrayToStructArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_StructArrayToStructArray>().Should().BeEquivalentTo(new TargetUser_StructArrayToStructArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_StructArrayToStructArray[] {
                    new TargetAddress_StructArrayToStructArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_StructArrayToStructArray
        {
            public string Name { get; set; }
            public SourceAddress_StructArrayToStructArray[] Addresses { get; set; }
        }
        internal struct SourceAddress_StructArrayToStructArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_StructArrayToStructArray
        {
            public string Name { get; set; }
            public TargetAddress_StructArrayToStructArray[] Addresses { get; set; }
        }
        internal struct TargetAddress_StructArrayToStructArray
        {
            public string City { get; set; }
        }
        #endregion

        #region ClassArrayToStructArray
        [Fact]
        public void ShouldConvertClassArrayToStructArray()
        {
            var user = new SourceUser_ClassArrayToStructArray
            {
                Name = "zhangsan",
                Addresses = new SourceAddress_ClassArrayToStructArray[]
                {
                    new SourceAddress_ClassArrayToStructArray{ City = "xi'an" }
                }
            };
            user.To<TargetUser_ClassArrayToStructArray>().Should().BeEquivalentTo(new TargetUser_ClassArrayToStructArray
            {
                Name = "zhangsan",
                Addresses = new TargetAddress_ClassArrayToStructArray[] {
                    new TargetAddress_ClassArrayToStructArray{ City ="xi'an"}
                }
            });
        }
        internal class SourceUser_ClassArrayToStructArray
        {
            public string Name { get; set; }
            public SourceAddress_ClassArrayToStructArray[] Addresses { get; set; }
        }
        internal class SourceAddress_ClassArrayToStructArray
        {
            public string City { get; set; }
        }

        internal class TargetUser_ClassArrayToStructArray
        {
            public string Name { get; set; }
            public TargetAddress_ClassArrayToStructArray[] Addresses { get; set; }
        }
        internal struct TargetAddress_ClassArrayToStructArray
        {
            public string City { get; set; }
        }
        #endregion


        #region AssignValue
        internal class SourceClass<T>
        {
            public T Value { get; set; }
        }
        internal class TargetClass<T>
        {
            public T Value { get; set; }
        }

        #region Int32ToInt32
        [Fact]
        public void ShouldConvertInt32ToInt32()
        {
            var source = new SourceClass<int>()
            {
                Value = 100,
            };
            source.To<TargetClass<int>>().Should().BeEquivalentTo(new TargetClass<int>
            {
                Value = 100
            });
        }
        #endregion

        #region Int32ToInt64 
        [Fact]
        public void ShouldConvertInt32ToInt64()
        {
            var source = new SourceClass<int>()
            {
                Value = 100,
            };
            source.To<TargetClass<long>>().Should().BeEquivalentTo(new TargetClass<long>
            {
                Value = 100
            });
        }
        #endregion

        #region Int32ToNullableInt32
        [Fact]
        public void ShouldConvertInt32ToNullableInt32()
        {
            var source = new SourceClass<int>()
            {
                Value = 100,
            };
            source.To<TargetClass<int?>>().Should().BeEquivalentTo(new TargetClass<int?>
            {
                Value = 100
            });
        }
        #endregion

        #region Int32ToNullableInt64
        [Fact]
        public void ShouldConvertInt32ToNullableInt64()
        {
            var source = new SourceClass<int>()
            {
                Value = 100,
            };
            source.To<TargetClass<long?>>().Should().BeEquivalentTo(new TargetClass<long?>
            {
                Value = 100
            });
        }
        #endregion

        #region Int32ToObject
        [Fact]
        public void ShouldConvertInt32ToObject()
        {
            var source = new SourceClass<int>()
            {
                Value = 100,
            };
            source.To<TargetClass<object>>().Should().BeEquivalentTo(new TargetClass<object>
            {
                Value = 100
            });
        }
        #endregion

        #region TheSameStruct
        [Fact]
        public void ShouldConvertTheSameStruct()
        {
            var source = new SourceClass<DateTime>()
            {
                Value = new DateTime(2023, 10, 15),
            };
            source.To<TargetClass<DateTime>>().Should().BeEquivalentTo(new TargetClass<DateTime>
            {
                Value = new DateTime(2023, 10, 15)
            });
        }
        #endregion


        #region StructToNullableStruct
        [Fact]
        public void ShouldConvertStructToNullableStruct()
        {
            var source = new SourceClass<DateTime>()
            {
                Value = new DateTime(2023, 10, 15),
            };
            source.To<TargetClass<DateTime?>>().Should().BeEquivalentTo(new TargetClass<DateTime?>
            {
                Value = new DateTime(2023, 10, 15)
            });
        }
        #endregion

        #region TheSameSubClass
        [Fact]
        public void ShouldConvertTheSameSubClass()
        {
            var source = new SourceClass<Value_TheSameSubClass>()
            {
                Value = new Value_TheSameSubClass { Value = "abc" }
            };
            var target = source.To<TargetClass<Value_TheSameSubClass>>();
            target.Value.Should().Be(source.Value);
        }
        internal class Value_TheSameSubClass
        {
            public string Value { get; set; }
        }
        #endregion

        #region ClassToBaseClass
        [Fact]
        public void ShouldConvertClassToBaseClass()
        {
            var source = new SourceClass<Value_ClassToBaseClass>()
            {
                Value = new Value_ClassToBaseClass { Value = "abc" }
            };
            var target = source.To<TargetClass<ValueParent_ClassToBaseClass>>();
            target.Value.Should().Be(source.Value);
        }
        internal class ValueParent_ClassToBaseClass
        {
            public string Value { get; set; }
        }
        internal class Value_ClassToBaseClass : ValueParent_ClassToBaseClass
        {
        }
        #endregion

        #region ClassToInterface

        [Fact]
        public void ShouldConvertClassToInteface()
        {
            var source = new SourceClass<Value_ClassToInterface>()
            {
                Value = new Value_ClassToInterface { Value = "abc" }
            };
            var target = source.To<TargetClass<IValue_ClassToInterface>>();
            target.Value.Should().Be(source.Value);
        }
        internal interface IValue_ClassToInterface
        {
            string Value { get; set; }
        }
        internal class Value_ClassToInterface : IValue_ClassToInterface
        {
            public string Value { get; set; }
        }

        #endregion

        #region Int32ArrayToInt32Array
        [Fact]
        public void ShouldConvertInt32ArrayToInt32Array()
        {
            var source = new SourceClass<int[]>()
            {
                Value = new int[] { 1, 3, 5 }
            };
            var target = source.To<TargetClass<int[]>>();
            target.Value.Should().BeSameAs(source.Value);
        }
        #endregion

        #region StringArrayToStringArray
        [Fact]
        public void ShouldConvertStringArrayToStringArray()
        {
            var source = new SourceClass<string[]>()
            {
                Value = new string[] { "a", "b", "c" }
            };
            var target = source.To<TargetClass<string[]>>();
            target.Value.Should().BeSameAs(source.Value);
        }
        #endregion

        #region StringArrayToStringList
        [Fact]
        public void ShouldConvertStringArrayToStringList()
        {
            var source = new SourceClass<string[]>()
            {
                Value = new string[] { "a", "b", "c" }
            };
            var target = source.To<TargetClass<List<string>>>();
            target.Should().BeEquivalentTo(new TargetClass<List<string>>
            {
                Value = new List<string> { "a", "b", "c" }
            });
        }
        #endregion

        #region Int32ArrayToInt64Array
        [Fact]
        public void ShouldConvertInt32ArrayToInt64Array()
        {
            var source = new SourceClass<int[]>()
            {
                Value = new int[] { 1, 3, 5 }
            };
            var target = source.To<TargetClass<long[]>>();
            target.Should().BeEquivalentTo(new TargetClass<long[]>
            {
                Value = new long[] { 1, 3, 5 }
            });
        }
        #endregion

        #region Int32ArrayToNullableInt32Array
        [Fact]
        public void ShouldConvertInt32ArrayToNullableInt32Array()
        {
            var source = new SourceClass<int[]>()
            {
                Value = new int[] { 1, 3, 5 }
            };
            var target = source.To<TargetClass<int?[]>>();
            target.Should().BeEquivalentTo(new TargetClass<int?[]>
            {
                Value = new int?[] { 1, 3, 5 }
            });
        }

        #endregion

        #region Int32ArrayToNullableInt64IList
        [Fact]
        public void ShouldConvertInt32ArrayToNullableInt64IList()
        {
            var source = new SourceClass<int[]>()
            {
                Value = new int[] { 1, 3, 5 }
            };
            var target = source.To<TargetClass<IList<long?>>>();
            target.Should().BeEquivalentTo(new TargetClass<IList<long?>>
            {
                Value = new long?[] { 1, 3, 5 }.ToList()
            });
        }

        #endregion


        #region TheSameClassArray
        [Fact]
        public void ShouldConvertTheSameClassArray()
        {
            var source = new SourceClass<Value_TheSameClassArray[]>()
            {
                Value = new Value_TheSameClassArray[] { }
            };
            var target = source.To<TargetClass<Value_TheSameClassArray[]>>();
            source.Value.Should().BeSameAs(target.Value);
        }
        internal class Value_TheSameClassArray
        {
            public int Value { get; set; }
        }
        #endregion

        #region TheSameStructArray
        [Fact]
        public void ShouldConvertTheSameStructArray()
        {
            var source = new SourceClass<Value_TheSameStructArray[]>()
            {
                Value = new Value_TheSameStructArray[] { }
            };
            var target = source.To<TargetClass<Value_TheSameStructArray[]>>();
            source.Value.Should().BeSameAs(target.Value);
        }

        internal struct Value_TheSameStructArray
        {
            public int Value { get; set; }
        }
        #endregion

        #region TheSameStructArrayToIList

        [Fact]
        public void ShouldConvertTheSameStructArrayToIList()
        {
            var source = new SourceClass<Value_TheSameStructArrayToIList[]>()
            {
                Value = new Value_TheSameStructArrayToIList[] { new Value_TheSameStructArrayToIList { Value = 1 } }
            };
            var target = source.To<TargetClass<IList<Value_TheSameStructArrayToIList>>>();
            target.Should().BeEquivalentTo(new TargetClass<IList<Value_TheSameStructArrayToIList>>
            {
                Value = new Value_TheSameStructArrayToIList[] { new Value_TheSameStructArrayToIList { Value = 1 } }
            });
        }

        internal struct Value_TheSameStructArrayToIList
        {
            public int Value { get; set; }
        }
        #endregion

        #region TheSameStructArrayToNullableList

        [Fact]
        public void ShouldConvertTheSameStructArrayToNullableList()
        {
            var source = new SourceClass<Value_TheSameStructArrayToNullableList[]>()
            {
                Value = new Value_TheSameStructArrayToNullableList[] { new Value_TheSameStructArrayToNullableList { Value = 1 } }
            };
            var target = source.To<TargetClass<List<Value_TheSameStructArrayToNullableList?>>>();
            target.Should().BeEquivalentTo(new TargetClass<List<Value_TheSameStructArrayToNullableList?>>
            {
                Value = new List<Value_TheSameStructArrayToNullableList?> { new Value_TheSameStructArrayToNullableList { Value = 1 } }
            });
        }

        internal struct Value_TheSameStructArrayToNullableList
        {
            public int Value { get; set; }
        }
        #endregion

        #region DictionaryStringStringToDictionaryStringString
        [Fact]
        public void ShouldConvertDictionaryStringStringToDictionaryStringString()
        {
            var source = new SourceClass<Dictionary<string, string>>()
            {
                Value = new Dictionary<string, string>
                {
                    ["a"] = "value1"
                }
            };
            var target = source.To<TargetClass<Dictionary<string, string>>>();
            target.Value.Should().BeSameAs(source.Value);
        }
        #endregion

        #region DictionaryInt32Int32ToDictionaryInt32Int32
        [Fact]
        public void ShouldConvertDictionaryInt32Int32ToDictionaryInt32Int32()
        {
            var source = new SourceClass<Dictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<Dictionary<int, int>>>();
            target.Value.Should().BeSameAs(source.Value);
        }

        #endregion

        #region IDictionaryInt32Int32ToDictionaryInt32Int32
        [Fact]
        public void ShouldConvertIDictionaryInt32Int32ToDictionaryInt32Int32()
        {
            var source = new SourceClass<IDictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<Dictionary<int, int>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, int>>
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            });
        }
        #endregion

        #region DictionaryInt32Int32ToIDictionaryInt32Int32
        [Fact]
        public void ShouldConvertDictionaryInt32Int32ToIDictionaryInt32Int32()
        {
            var source = new SourceClass<Dictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<IDictionary<int, int>>>();
            target.Value.Should().BeSameAs(source.Value);
        }
        #endregion

        #region IDictionaryInt32Int32ToIDictionaryInt32Int32
        [Fact]
        public void ShouldConvertIDictionaryInt32Int32ToIDictionaryInt32Int32()
        {
            var source = new SourceClass<IDictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<IDictionary<int, int>>>();
            target.Value.Should().BeSameAs(source.Value);
        }
        #endregion

        #region DictionaryInt32Int32ToDictionaryNullableInt32Int32
        [Fact]
        public void ShouldConvertDictionaryInt32Int32ToDictionaryNullableInt32Int32()
        {
            var source = new SourceClass<Dictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<Dictionary<int?, int>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int?, int>>
            {
                Value = new Dictionary<int?, int>
                {
                    [1] = 1
                }
            });
        }
        #endregion

        #region DictionaryInt32Int32ToDictionaryInt32NullableInt32
        [Fact]
        public void ShouldConvertDictionaryInt32Int32ToDictionaryInt32NullableInt32()
        {
            var source = new SourceClass<Dictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<Dictionary<int, int?>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, int?>>
            {
                Value = new Dictionary<int, int?>
                {
                    [1] = 1
                }
            });
        }
        #endregion

        #region DictionaryInt32Int32ToDictionaryNullableInt32NullableInt32
        [Fact]
        public void ShouldConvertDictionaryInt32Int32ToDictionaryNullableInt32NullableInt32()
        {
            var source = new SourceClass<Dictionary<int, int>>()
            {
                Value = new Dictionary<int, int>
                {
                    [1] = 1
                }
            };
            var target = source.To<TargetClass<Dictionary<int?, int?>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int?, int?>>
            {
                Value = new Dictionary<int?, int?>
                {
                    [1] = 1
                }
            });
        }
        #endregion

        #region DictionaryInt32DateTimeToDictionaryInt32NullableDateTime
        [Fact]
        public void ShouldConvertDictionaryInt32DateTimeToDictionaryInt32NullableDateTime()
        {
            var source = new SourceClass<Dictionary<int, DateTime>>()
            {
                Value = new Dictionary<int, DateTime>
                {
                    [1] = new DateTime(2023, 10, 16)
                }
            };
            var target = source.To<TargetClass<Dictionary<int, DateTime?>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, DateTime?>>
            {
                Value = new Dictionary<int, DateTime?>
                {
                    [1] = new DateTime(2023, 10, 16)
                }
            });
        }
        #endregion

        #region DictionaryInt32ObjectToDictionaryInt32Object2
        [Fact]
        public void ShouldConvertDictionaryInt32ObjectToDictionaryInt32Object2()
        {
            var source = new SourceClass<Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Object2>>()
            {
                Value = new Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Object2>
                {
                    [1] = new SourceRecord_DictionaryInt32ObjectToDictionaryInt32Object2 { Value = "abc" },
                }
            };
            var target = source.To<TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2>>
            {
                Value = new Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2>
                {
                    [1] = new TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2 { Value = "abc" }
                }
            });
        }
        internal record SourceRecord_DictionaryInt32ObjectToDictionaryInt32Object2
        {
            public string Value { get; set; }
        }
        internal record TargetRecord_DictionaryInt32ObjectToDictionaryInt32Object2
        {
            public string Value { get; init; }
        }
        #endregion


        #region DictionaryInt32ObjectToDictionaryInt32Struct
        [Fact]
        public void ShouldConvertDictionaryInt32ObjectToDictionaryInt32Struct()
        {
            var source = new SourceClass<Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Struct>>()
            {
                Value = new Dictionary<int, SourceRecord_DictionaryInt32ObjectToDictionaryInt32Struct>
                {
                    [1] = new SourceRecord_DictionaryInt32ObjectToDictionaryInt32Struct { Value = "abc" },
                }
            };
            var target = source.To<TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct>>
            {
                Value = new Dictionary<int, TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct>
                {
                    [1] = new TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct { Value = "abc" }
                }
            });
        }
        internal record SourceRecord_DictionaryInt32ObjectToDictionaryInt32Struct
        {
            public string Value { get; set; }
        }
        internal struct TargetRecord_DictionaryInt32ObjectToDictionaryInt32Struct
        {
            public string Value { get; init; }
        }
        #endregion


        #region DictionaryInt32StructToDictionaryInt32NullableStruct
        [Fact]
        public void ShouldConvertDictionaryInt32StructToDictionaryInt32NullableStruct()
        {
            var source = new SourceClass<Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct>>()
            {
                Value = new Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct>
                {
                    [1] = new Struct_DictionaryInt32StructToDictionaryInt32NullableStruct { Value = "abc" },
                }
            };
            var target = source.To<TargetClass<Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct?>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct?>>
            {
                Value = new Dictionary<int, Struct_DictionaryInt32StructToDictionaryInt32NullableStruct?>
                {
                    [1] = new Struct_DictionaryInt32StructToDictionaryInt32NullableStruct { Value = "abc" }
                }
            });
        }
        internal struct Struct_DictionaryInt32StructToDictionaryInt32NullableStruct
        {
            public string Value { get; set; }
        }
        #endregion


        #region DictionaryObjectInt32ToDictionaryObject2Int64
        [Fact]
        public void ShouldConvertDictionaryObjectInt32ToDictionaryObject2Int64()
        {
            var source = new SourceClass<Dictionary<SourceRecord_DictionaryObjectInt32ToDictionaryObject2Int64, int>>()
            {
                Value = new Dictionary<SourceRecord_DictionaryObjectInt32ToDictionaryObject2Int64, int>
                {
                    [new SourceRecord_DictionaryObjectInt32ToDictionaryObject2Int64 { Value = "abc" }] = 123,
                }
            };
            var target = source.To<TargetClass<Dictionary<TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64, long>>>();
            target.Should().BeEquivalentTo(new TargetClass<Dictionary<TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64, long>>
            {
                Value = new Dictionary<TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64, long>
                {
                    [new TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64 { Value = "abc" }] = 123L,
                }
            });
        }
        internal record SourceRecord_DictionaryObjectInt32ToDictionaryObject2Int64
        {
            public string Value { get; set; }
        }
        internal record TargetRecord_DictionaryObjectInt32ToDictionaryObject2Int64
        {
            public string Value { get; init; }
        }
        #endregion

        #endregion

        #region Record
        #region ClassToRecord
        internal class SourceUser_ClassToRecord
        {
            public string Name { get; set; }
        }


        internal record TargetUser_ClassToRecord
        {
            public string Name { get; set; }
        }
        [Fact]
        public void ShouldConvertClassToRecord()
        {
            var source = new SourceUser_ClassToRecord
            {
                Name = "zhangsan"
            };
            source.To<TargetUser_ClassToRecord>().Should().BeEquivalentTo(new TargetUser_ClassToRecord
            {
                Name = "zhangsan"
            });
        }
        #endregion

        #region RecordToClass
        internal class SourceUser_RecordToClass
        {
            public string Name { get; set; }
        }


        internal record TargetUser_RecordToClass
        {
            public string Name { get; set; }
        }
        [Fact]
        public void ShouldConvertRecordToClass()
        {
            var source = new SourceUser_RecordToClass
            {
                Name = "zhangsan"
            };
            source.To<TargetUser_RecordToClass>().Should().BeEquivalentTo(new TargetUser_RecordToClass
            {
                Name = "zhangsan"
            });
        }
        #endregion
        #endregion


        #region InitOnly

        #region ClassToInitOnlyRecord
        internal class SourceUser_ClassToInitOnlyRecord
        {
            public string Name { get; set; }
        }


        internal record TargetUser_ClassToInitOnlyRecord
        {
            public string Name { get; init; }
        }
        [Fact]
        public void ShouldConvertClassToInitOnlyRecord()
        {
            var source = new SourceUser_ClassToInitOnlyRecord
            {
                Name = "zhangsan"
            };
            source.To<TargetUser_ClassToInitOnlyRecord>().Should().BeEquivalentTo(new TargetUser_ClassToInitOnlyRecord
            {
                Name = "zhangsan"
            });
        }
        #endregion

        #region ClassToInitOnlyClass
        internal class SourceUser_ClassToInitOnlyClass
        {
            public string Name { get; set; }
        }


        internal class TargetUser_ClassToInitOnlyClass
        {
            public string Name { get; init; }
        }
        [Fact]
        public void ShouldConvertClassToInitOnlyClass()
        {
            var source = new SourceUser_ClassToInitOnlyClass
            {
                Name = "zhangsan"
            };
            source.To<TargetUser_ClassToInitOnlyClass>().Should().BeEquivalentTo(new TargetUser_ClassToInitOnlyClass
            {
                Name = "zhangsan"
            });
        }
        #endregion
        #endregion

        #region WithPostHandler
        internal class SourceUser_WithPostHandler
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        internal class TargetUser_WithPostHandler
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
        }
        [Fact]
        public void ShouldConvertWithPostHandler()
        {
            var source = new SourceUser_WithPostHandler
            {
                FirstName = "zhang",
                LastName = "san"
            };
            source.To<TargetUser_WithPostHandler>(t => { t.FullName = t.FirstName + t.LastName; }).Should().BeEquivalentTo(new TargetUser_WithPostHandler
            {
                FirstName = "zhang",
                LastName = "san",
                FullName = "zhangsan"
            });
        }
        #endregion

        #region WithCustomMappings
        internal class SourceUser_WithCustomMappings
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        internal class TargetUser_WithCustomMappings
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
        }
        [Fact]
        public void ShouldConvertWithCustomMappings()
        {
            var source = new SourceUser_WithCustomMappings
            {
                FirstName = "zhang",
                LastName = "san"
            };
            source.To<TargetUser_WithCustomMappings>().Should().BeEquivalentTo(new TargetUser_WithCustomMappings
            {
                FirstName = "zhang",
                LastName = "san",
                FullName = "zhangsan"
            });
        }
        #endregion

        #region WithIgnoreProperties
        internal class SourceUser_WithIgnoreProperties
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        internal class TargetUser_WithIgnoreProperties
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        [Fact]
        public void ShouldConvertWithIgnoreProperties()
        {
            var source = new SourceUser_WithIgnoreProperties
            {
                FirstName = "zhang",
                LastName = "san"
            };
            source.To<TargetUser_WithIgnoreProperties>().Should().BeEquivalentTo(new TargetUser_WithIgnoreProperties
            {
                FirstName = "zhang",
                LastName = null,
            });
        }
        #endregion



        //private static T To<T>(global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.SourceUser_ArrayToImmutableArray source) where T : new()
        //{
        //    if (source == null) return default;
        //    if (typeof(T) == typeof(global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.TargetUser_ArrayToImmutableArray))
        //    {
        //        return (T)(object)ToFlyTiger_IntegrationTest_Mapper_ConvertSingleObjectTest_TargetUser_ArrayToImmutableArray(source);
        //    }
        //    throw new NotSupportedException($"Can not convert '{typeof(global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.SourceUser_ArrayToImmutableArray)}' to '{typeof(T)}'.");
        //}
        //private static global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.TargetUser_ArrayToImmutableArray ToFlyTiger_IntegrationTest_Mapper_ConvertSingleObjectTest_TargetUser_ArrayToImmutableArray(this global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.SourceUser_ArrayToImmutableArray source)
        //{
        //    if (source == null) return default;
        //    return new global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.TargetUser_ArrayToImmutableArray
        //    {
        //        Name = source.Name,
        //        Addresses = source.Addresses == null ? default : source.Addresses.Select(p => p == null ? default(global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.TargetAddress_ArrayToImmutableArray) : new global::FlyTiger.IntegrationTest.Mapper.ConvertSingleObjectTest.TargetAddress_ArrayToImmutableArray
        //        {
        //            City = p.City,
        //        }).ToImmutableArray(),
        //    };
        //}
    }
}
