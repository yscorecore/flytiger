using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;

namespace FlyTiger.IntegrationTest.Mapper
{
    [Mapper(typeof(SourceUser_UpdateDictionaryToDictionary), typeof(TargetUser_UpdateDictionaryToDictionary), MapperType = MapperType.BatchUpdate)]
    [Mapper(typeof(SourceUser_UpdateDictionaryToDictionaryWithAction), typeof(TargetUser_UpdateDictionaryToDictionaryWithAction), MapperType = MapperType.BatchUpdate)]
    public class BatchUpdateTest
    {
        #region  UpdateDictionaryToDictionary
        [Fact]
        public void Should_UpdateDictionaryToDictionary()
        {
            var source = new Dictionary<string, SourceUser_UpdateDictionaryToDictionary>
            {
                ["1"] = new SourceUser_UpdateDictionaryToDictionary { Name = "user1" },
                ["2"] = new SourceUser_UpdateDictionaryToDictionary { Name = "user2" }
            };
            var target = new Dictionary<string, TargetUser_UpdateDictionaryToDictionary>
            {
                ["2"] = new TargetUser_UpdateDictionaryToDictionary { Name = "user2--" },
                ["3"] = new TargetUser_UpdateDictionaryToDictionary { Name = "user3" }
            };
            source.To(target);
            target.Should().HaveCount(2);
            target["2"].Name.Should().Be("user2");
            target["1"].Name.Should().Be("user1");
            target.Should().ContainKey("1");
        }

        internal record SourceUser_UpdateDictionaryToDictionary
        {
            public string Name { get; set; }
        }

        internal record TargetUser_UpdateDictionaryToDictionary
        {
            public string Name { get; set; }
        }
        #endregion
        #region  UpdateDictionaryToDictionaryWithAction
        [Fact]
        public void Should_UpdateDictionaryToDictionaryWithAction()
        {
            var removeAction = Mock.Of<Action<object>>();
            var addAction = Mock.Of<Action<object>>();
            var source = new Dictionary<string, SourceUser_UpdateDictionaryToDictionaryWithAction>
            {
                ["1"] = new SourceUser_UpdateDictionaryToDictionaryWithAction { Name = "user1" },
                ["2"] = new SourceUser_UpdateDictionaryToDictionaryWithAction { Name = "user2" }
            };
            var target = new Dictionary<string, TargetUser_UpdateDictionaryToDictionaryWithAction>
            {
                ["2"] = new TargetUser_UpdateDictionaryToDictionaryWithAction { Name = "user2--" },
                ["3"] = new TargetUser_UpdateDictionaryToDictionaryWithAction { Name = "user3" }
            };
            source.To(target, removeAction, addAction);
            target.Should().HaveCount(2);
            target["2"].Name.Should().Be("user2");
            target["1"].Name.Should().Be("user1");
            target.Should().ContainKey("1");

            Mock.Get(removeAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Once());
            Mock.Get(addAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Once());
        }

        internal record SourceUser_UpdateDictionaryToDictionaryWithAction
        {
            public string Name { get; set; }
        }

        internal record TargetUser_UpdateDictionaryToDictionaryWithAction
        {
            public string Name { get; set; }
        }
        #endregion
    }
}
