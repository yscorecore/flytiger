using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using static FlyTiger.IntegrationTest.CopySingleObjectTest;

namespace FlyTiger.IntegrationTest.Mapper
{
    [Mapper(typeof(User1), typeof(TargetUser1))]
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

        public class User1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class TargetUser1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
