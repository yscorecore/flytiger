using System.Collections.Immutable;
using FluentAssertions;

namespace FlyTiger.IntegrationTest.Mapper
{

    [Mapper(typeof(SourceUser_ConvertDictionaryToDictionary), typeof(TargetUser_ConvertDictionaryToDictionary), MapperType = MapperType.Convert)]
    [Mapper(typeof(SourceUser_ConvertDictionaryToDictionaryWithPostAction), typeof(TargetUser_ConvertDictionaryToDictionaryWithPostAction), MapperType = MapperType.Convert)]
    public class ConvertDictionaryTest
    {
        #region  ConvertDictionaryToDictionary
        [Fact]
        public void Should_ConvertDictionaryToDictionary()
        {
            var source = new Dictionary<string, SourceUser_ConvertDictionaryToDictionary>
            {
                ["1"] = new SourceUser_ConvertDictionaryToDictionary { Name = "user1" },
                ["2"] = new SourceUser_ConvertDictionaryToDictionary { Name = "user2" }
            };

            var res = source.To<string, TargetUser_ConvertDictionaryToDictionary>();
            res.Should().HaveCount(2);
            res["2"].Name.Should().Be("user2");
            res["1"].Name.Should().Be("user1");
        }

        internal record SourceUser_ConvertDictionaryToDictionary
        {
            public string Name { get; set; }
        }

        internal record TargetUser_ConvertDictionaryToDictionary
        {
            public string Name { get; set; }
        }
        #endregion 

        #region  ConvertDictionaryToDictionaryWithPostAction
        [Fact]
        public void Should_ConvertDictionaryToDictionaryWithPostAction()
        {
            var source = new Dictionary<string, SourceUser_ConvertDictionaryToDictionaryWithPostAction>
            {
                ["1"] = new SourceUser_ConvertDictionaryToDictionaryWithPostAction { Name = "user1" },
                ["2"] = new SourceUser_ConvertDictionaryToDictionaryWithPostAction { Name = "user2" }
            };

            var res = source.To<string, TargetUser_ConvertDictionaryToDictionaryWithPostAction>(t => t.Name += "...");
            res.Should().HaveCount(2);
            res["2"].Name.Should().Be("user2...");
            res["1"].Name.Should().Be("user1...");
        }

        internal record SourceUser_ConvertDictionaryToDictionaryWithPostAction
        {
            public string Name { get; set; }
        }

        internal record TargetUser_ConvertDictionaryToDictionaryWithPostAction
        {
            public string Name { get; set; }
        }
        #endregion 
    }


}
