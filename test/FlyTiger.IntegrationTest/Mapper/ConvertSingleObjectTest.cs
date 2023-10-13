using FluentAssertions;

namespace FlyTiger.IntegrationTest.Mapper
{

    [Mapper(typeof(SourceUser_ArrayToIList), typeof(TargetUser_ArrayToIList))]
    [Mapper(typeof(SourceUser_ArrayToICollection), typeof(TargetUser_ArrayToICollection))]
    [Mapper(typeof(SourceUser_ArrayToIEnumerable), typeof(TargetUser_ArrayToIEnumerable))]
    [Mapper(typeof(SourceUser_ArrayToList), typeof(TargetUser_ArrayToList))]
    [Mapper(typeof(SourceUser_ArrayToIQueryable), typeof(TargetUser_ArrayToIQueryable))]
    [Mapper(typeof(SourceUser_ArrayToArray), typeof(TargetUser_ArrayToArray))]

    [Mapper(typeof(SourceUser_ListToIList), typeof(TargetUser_ListToIList))]
    [Mapper(typeof(SourceUser_ListToICollection), typeof(TargetUser_ListToICollection))]
    [Mapper(typeof(SourceUser_ListToIEnumerable), typeof(TargetUser_ListToIEnumerable))]
    [Mapper(typeof(SourceUser_ListToList), typeof(TargetUser_ListToList))]
    [Mapper(typeof(SourceUser_ListToIQueryable), typeof(TargetUser_ListToIQueryable))]
    [Mapper(typeof(SourceUser_ListToArray), typeof(TargetUser_ListToArray))]

    [Mapper(typeof(SourceUser_IListToIList), typeof(TargetUser_IListToIList))]
    [Mapper(typeof(SourceUser_IListToICollection), typeof(TargetUser_IListToICollection))]
    [Mapper(typeof(SourceUser_IListToIEnumerable), typeof(TargetUser_IListToIEnumerable))]
    [Mapper(typeof(SourceUser_IListToList), typeof(TargetUser_IListToList))]
    [Mapper(typeof(SourceUser_IListToIQueryable), typeof(TargetUser_IListToIQueryable))]
    [Mapper(typeof(SourceUser_IListToArray), typeof(TargetUser_IListToArray))]

    [Mapper(typeof(SourceUser_ICollectionToIList), typeof(TargetUser_ICollectionToIList))]
    [Mapper(typeof(SourceUser_ICollectionToICollection), typeof(TargetUser_ICollectionToICollection))]
    [Mapper(typeof(SourceUser_ICollectionToIEnumerable), typeof(TargetUser_ICollectionToIEnumerable))]
    [Mapper(typeof(SourceUser_ICollectionToList), typeof(TargetUser_ICollectionToList))]
    [Mapper(typeof(SourceUser_ICollectionToIQueryable), typeof(TargetUser_ICollectionToIQueryable))]
    [Mapper(typeof(SourceUser_ICollectionToArray), typeof(TargetUser_ICollectionToArray))]

    [Mapper(typeof(SourceUser_IEnumerableToIList), typeof(TargetUser_IEnumerableToIList))]
    [Mapper(typeof(SourceUser_IEnumerableToICollection), typeof(TargetUser_IEnumerableToICollection))]
    [Mapper(typeof(SourceUser_IEnumerableToIEnumerable), typeof(TargetUser_IEnumerableToIEnumerable))]
    [Mapper(typeof(SourceUser_IEnumerableToList), typeof(TargetUser_IEnumerableToList))]
    [Mapper(typeof(SourceUser_IEnumerableToIQueryable), typeof(TargetUser_IEnumerableToIQueryable))]
    [Mapper(typeof(SourceUser_IEnumerableToArray), typeof(TargetUser_IEnumerableToArray))]

    [Mapper(typeof(SourceUser_IQueryableToIList), typeof(TargetUser_IQueryableToIList))]
    [Mapper(typeof(SourceUser_IQueryableToICollection), typeof(TargetUser_IQueryableToICollection))]
    [Mapper(typeof(SourceUser_IQueryableToIEnumerable), typeof(TargetUser_IQueryableToIEnumerable))]
    [Mapper(typeof(SourceUser_IQueryableToList), typeof(TargetUser_IQueryableToList))]
    [Mapper(typeof(SourceUser_IQueryableToIQueryable), typeof(TargetUser_IQueryableToIQueryable))]
    [Mapper(typeof(SourceUser_IQueryableToArray), typeof(TargetUser_IQueryableToArray))]

    [Mapper(typeof(SourceUser_StructToClass), typeof(TargetUser_StructToClass))]
    //[Mapper(typeof(SourceUser_ClassToStruct), typeof(TargetUser_ClassToStruct))]
    //[Mapper(typeof(SourceUser_StructToStruct), typeof(TargetUser_StructToStruct))]
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

        //#region ClassToStruct
        //[Fact]
        //public void ShouldConvertClassToStruct()
        //{
        //    var user = new SourceUser_ClassToStruct
        //    {
        //        Name = "zhangsan",
        //    };
        //    user.To<TargetUser_ClassToStruct>().Should().BeEquivalentTo(new TargetUser_ClassToStruct
        //    {
        //        Name = "zhangsan",
        //    });
        //}

        //internal class SourceUser_ClassToStruct
        //{
        //    public string Name { get; set; }
        //}


        //internal struct TargetUser_ClassToStruct
        //{
        //    public string Name { get; set; }
        //}


        //#endregion

        //#region StructToStruct
        //[Fact]
        //public void ShouldConvertStructToStruct()
        //{
        //    var user = new SourceUser_StructToStruct
        //    {
        //        Name = "zhangsan",
        //    };
        //    user.To<TargetUser_StructToStruct>().Should().BeEquivalentTo(new TargetUser_StructToStruct
        //    {
        //        Name = "zhangsan",
        //    });
        //}

        //internal struct SourceUser_StructToStruct
        //{
        //    public string Name { get; set; }
        //}


        //internal struct TargetUser_StructToStruct
        //{
        //    public string Name { get; set; }
        //}


        //#endregion

        #region SubStructToClass
        #endregion
        #region SubClassToStruct
        #endregion
        #region SubStructToStruct
        #endregion

        #region SturctArrayToClassArray
        #endregion
        #region SturctArrayToStructArray
        #endregion
        #region ClassArrayToCStructArray
        #endregion


    }
}
