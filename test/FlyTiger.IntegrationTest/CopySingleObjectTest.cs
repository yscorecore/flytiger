using FluentAssertions;

namespace FlyTiger.IntegrationTest
{
    [Mapper(typeof(User1), typeof(TargetUser1))]
    [Mapper(typeof(User2), typeof(TargetUser2),
        CustomMappings = new[] { $"{nameof(TargetUser2.FullName)} = $.{nameof(User2.FirstName)} + $.{nameof(User2.LastName)}" })]
    [Mapper(typeof(User3), typeof(TargetUser3), IgnoreTargetProperties = new[] { nameof(TargetUser3.Age) })]
    [Mapper(typeof(User4), typeof(TargetUser4))]
    public class CopySingleObjectTest
    {
        [Fact]
        public void ShouldCopySampleProperty()
        {
            var user = new User1()
            {
                Name = "zhangsan",
                Age = 30,
            };
            var target = new TargetUser1();
            user.To(target);
            target.Name.Should().Be("zhangsan");
            target.Age.Should().Be(30);
        }
        [Fact]
        public void ShouldCustomeProperty()
        {
            var user = new User2()
            {
                FirstName = "zhang",
                LastName = "san",
                Age = 30,
            };
            var target = new TargetUser2();
            user.To(target);
            target.FullName.Should().Be("zhangsan");
            target.Age.Should().Be(30);
        }
        [Fact]
        public void ShouldIgnoreProperty()
        {
            var user = new User3()
            {
                Name = "zhangsan",
                Age = 30,
            };
            var target = new TargetUser3() { Age = 5 };
            user.To(target);
            target.Name.Should().Be("zhangsan");
            target.Age.Should().Be(5);
        }
        [Fact]
        public void ShouldCopySubObjectValue()
        {
            var user = new User4()
            {
                Address = new Address4 { City = "beijing" }
            };
            var targetAddress = new TargetAddress4 { City = "xi'an" };
            var target = new TargetUser4() { Address = targetAddress };
            user.To(target);
            target.Address.Should().Be(targetAddress);
            target.Address.City.Should().Be("beijing");
        }

        public class User1
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class TargetUser1
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class User2
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
        }
        public class TargetUser2
        {
            public string FullName { get; set; }
            public int Age { get; set; }
        }

        public class User3
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class TargetUser3
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class User4
        {
            public Address4 Address { get; set; }
        }
        public class Address4
        {
            public string City { get; set; }
        }
        public class TargetUser4
        {
            public TargetAddress4 Address { get; set; }

        }
        public class TargetAddress4
        {
            public string City { get; set; }
        }
    }
}
