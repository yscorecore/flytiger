using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Moq;

namespace FlyTiger.IntegrationTest.Mapper
{
    [Mapper(typeof(User1), typeof(TargetUser1))]
    [Mapper(typeof(User2), typeof(TargetUser2))]
    public class UpdateCollectionTest
    {
        [Fact]
        public void ShouldAppendUserArrayToTarget()
        {
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id =4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Append);
            target.Should().HaveCount(5);
            target.Where(p => p.Id == 2).Should().HaveCount(2);
        }
        [Fact]
        public void ShouldInvokeCallbackWhenAppendUserArrayToTarget()
        {
            var removeAction = Mock.Of<Action<object>>();
            var addAction = Mock.Of<Action<object>>();
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id =4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Append, removeAction, addAction);

            Mock.Get(removeAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Never());
            Mock.Get(addAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldMergeUserArrayToTarget()
        {
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id =4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Merge);
            target.Should().HaveCount(4);
            target.SingleOrDefault(p => p.Id == 2).Should().BeEquivalentTo(new TargetUser1 { Id = 2, Name = "lisi", Age = 15 });
            target.SingleOrDefault(p => p.Id == 4).Should().BeEquivalentTo(new TargetUser1 { Id = 4, Name = "wangmazi", Age = 13 });
        }
        [Fact]
        public void ShouldInvokeCallbackWhenMergeUserArrayToTarget()
        {
            var removeAction = Mock.Of<Action<object>>();
            var addAction = Mock.Of<Action<object>>();
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id =4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Merge, removeAction, addAction);

            Mock.Get(removeAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Never());
            Mock.Get(addAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Exactly(1));
        }
        [Fact]
        public void ShouldUpdateUserArrayToTarget()
        {
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id = 4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Update);
            target.Should().HaveCount(2);
            target.SingleOrDefault(p => p.Id == 2).Should().BeEquivalentTo(new TargetUser1 { Id = 2, Name = "lisi", Age = 15 });
        }
        [Fact]
        public void ShouldInvokeCallbackWhenUpdateUserArrayToTarget()
        {
            var removeAction = Mock.Of<Action<object>>();
            var addAction = Mock.Of<Action<object>>();
            var source = new[] {
                new User1 { Id = 2, Name = "lisi", Age = 15 },
                new User1 { Id =4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Update, removeAction, addAction);

            Mock.Get(removeAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Exactly(2));
            Mock.Get(addAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Exactly(1));
        }

        [Fact]
        public void ShouldUpdateWhenUseCustomSourceKey()
        {
            var source = new[] {
                new User1 { Id = 5, Name = "lisi", Age = 15 },
                new User1 { Id = 4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Update, p => p.Name);
            target.Should().HaveCount(2);
            target.SingleOrDefault(p => p.Id == 5).Should().BeEquivalentTo(new TargetUser1 { Id = 5, Name = "lisi", Age = 15 });
        }
        [Fact]
        public void ShouldUpdateWhenUseCustomSourceKeyAndTargetKey()
        {
            var source = new[] {
                new User1 { Id = 5, Name = "lisi", Age = 15 },
                new User1 { Id = 4, Name = "wangmazi", Age = 13 }
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Update, p => p.Name, p => p.Name);
            target.Should().HaveCount(2);
            target.SingleOrDefault(p => p.Id == 5).Should().BeEquivalentTo(new TargetUser1 { Id = 5, Name = "lisi", Age = 15 });
        }
        

        [Fact]
        public void ShouldUpdateWhenUseCustomSourceKeyAndTargetKeyAndKeyIsObject()
        {
            var source = new[] {
                new User1 { Id = 5, Name = "lisi", Age = 15 },
                new User1 { Id = 4, Name = "wangmazi", Age = 13 },
                new User1 { Id = 6, Name = "lisi", Age = 14 },
            };
            var target = new List<TargetUser1> {
                new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser1{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser1{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Merge, p => new { p.Age, p.Name }, p => new { p.Age, p.Name });
            target.Should().HaveCount(5)
                .And.BeEquivalentTo(new List<TargetUser1>
                {
                    new TargetUser1{ Id=1, Name="zhangsan", Age=12 },
                    new TargetUser1{ Id=6, Name="lisi", Age=14 } ,
                    new TargetUser1{ Id=3, Name="wangwu", Age=13 },
                    new TargetUser1 { Id = 5, Name = "lisi", Age = 15 },
                    new TargetUser1 { Id = 4, Name = "wangmazi", Age = 13 },

                }, o => o.WithoutStrictOrdering());
        }
        [Fact]
        public void ShouldUpdateWhenDefinedMultiKeys()
        {
            var source = new[] {
                new User2 { Id = 5, Name = "lisi", Age = 15 },
                new User2 { Id = 4, Name = "wangmazi", Age = 13 },
                new User2 { Id = 6, Name = "lisi", Age = 14 },
            };
            var target = new List<TargetUser2> {
                new TargetUser2{ Id=1, Name="zhangsan", Age=12 },
                new TargetUser2{ Id=2, Name="lisi", Age=14 } ,
                new TargetUser2{ Id=3, Name="wangwu", Age=13 }
            };
            source.To(target, CollectionUpdateMode.Merge);
            target.Should().HaveCount(5)
                .And.BeEquivalentTo(new List<TargetUser2>
                {
                    new TargetUser2{ Id=1, Name="zhangsan", Age=12 },
                    new TargetUser2{ Id=6, Name="lisi", Age=14 } ,
                    new TargetUser2{ Id=3, Name="wangwu", Age=13 },
                    new TargetUser2 { Id = 5, Name = "lisi", Age = 15 },
                    new TargetUser2 { Id = 4, Name = "wangmazi", Age = 13 },

                }, o => o.WithoutStrictOrdering());
        }
        public record User1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public record TargetUser1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public record User2
        {
            public int Id { get; set; }
            [Key]
            public string Name { get; set; }
            [Key]
            public int Age { get; set; }
        }
        public record TargetUser2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
