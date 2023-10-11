using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Moq;

namespace FlyTiger.IntegrationTest
{
    [Mapper(typeof(User1), typeof(TargetUser1))]
    [Mapper(typeof(User2), typeof(TargetUser2),
        CustomMappings = new[] { $"{nameof(TargetUser2.FullName)} = $.{nameof(User2.FirstName)} + $.{nameof(User2.LastName)}" })]
    [Mapper(typeof(User3), typeof(TargetUser3), IgnoreTargetProperties = new[] { nameof(TargetUser3.Age) })]
    [Mapper(typeof(User4), typeof(TargetUser4))]
    [Mapper(typeof(User5), typeof(TargetUser5))]
    [Mapper(typeof(User6), typeof(TargetUser6))]
    [Mapper(typeof(User7), typeof(TargetUser7))]
    [Mapper(typeof(User8), typeof(TargetUser8))]
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
        public void ShouldCopyCustomeProperty()
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
        public void ShouldSkipThosePropertiesWhenDefineIgnoreTargetProperties()
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
        [Fact]
        public void ShouldCopyNativePropertyValue()
        {
            var user = new User5()
            {
                Address = new Address5 { City = "beijing" }
            };

            var target = new TargetUser5() { };
            user.To(target);
            target.AddressCity.Should().Be("beijing");
        }
        [Fact]
        public void ShouldAddNewItemsWhenTargetItemsIsNull()
        {
            var user = new User6()
            {
                Address = new Address6[] {
                    new Address6{ Id=2, City="beijing"}
                }
            };
            var target = new TargetUser6();
            user.To(target);
            target.Address.Should().HaveCount(1);
            target.Address.First().Should().BeEquivalentTo(new TargetAddress6 { Id = 2, City = "beijing" });
        }
        [Fact]
        public void ShouldBatchUpdateItems()
        {
            var user = new User6()
            {
                Address = new Address6[] {
                    new Address6{ Id=2, City="beijing"}  ,
                    new Address6{ Id=3, City="nanjing"}  ,
                    new Address6{ Id=4, City="wuhan"}
                }
            };
            var addAction = Moq.Mock.Of<Action<object>>();
            var removeActon = Moq.Mock.Of<Action<object>>();
            var xianAddress = new TargetAddress6 { Id = 2, City = "xi'an" };
            var target = new TargetUser6() { Address = new List<TargetAddress6>() { xianAddress, new TargetAddress6 { Id = 1, City = "shanghai" } } };
            user.To(target, removeActon, addAction);
            target.Address.Should().HaveCount(3)
                .And.BeEquivalentTo(new List<TargetAddress6>
                {
                    new TargetAddress6 { Id=2, City ="beijing"},
                    new TargetAddress6 { Id=3, City ="nanjing"} ,
                    new TargetAddress6 { Id=4, City ="wuhan"} ,
                });
            //在原有对象上更新
            target.Address.First().Should().Be(xianAddress);
            Mock.Get(addAction).Verify(t => t.Invoke(It.IsAny<object>()), Times.Exactly(2));
            Mock.Get(removeActon).Verify(t => t.Invoke(It.IsAny<object>()), Times.Once);

        }
        [Fact]
        public void ShouldUpdateTargetById()
        {
            var user = new User7()
            {
                Address = new Address7[] {
                    new Address7{ Id = 1,  Key1="1",  City="beijing"}  ,
                    new Address7{ Id =2, Key1="2", City="nanjing"}  ,
                    new Address7{ Id=3, Key1="3", City="wuhan"}
                }
            };
            var targetAddress1 = new TargetAddress7 { Id = 3, Key1 = "1", City = "xi'an" };
            var targetAddress2 = new TargetAddress7 { Id = 2, Key1 = "3", City = "shanghai" };
            var target = new TargetUser7()
            {
                Address = new List<TargetAddress7>()
                {
                    targetAddress1,
                    targetAddress2
                }
            };
            user.To(target);
            targetAddress1.Should().BeEquivalentTo(new TargetAddress7 { Id = 3, Key1 = "3", City = "wuhan" });
            targetAddress2.Should().BeEquivalentTo(new TargetAddress7 { Id = 2, Key1 = "2", City = "nanjing" });
        }
        [Fact]
        public void ShouldUpdateTargetBySourceKey()
        {
            var user = new User8()
            {
                Address = new Address8[] {
                    new Address8{ Id = 1,  Key1="1",  City="beijing"}  ,
                    new Address8{ Id =2, Key1="2", City="nanjing"}  ,
                    new Address8{ Id=3, Key1="3", City="wuhan"}
                }
            };
            var targetAddress1 = new TargetAddress8 { Id = 3, Key1 = "1", City = "xi'an" };
            var targetAddress2 = new TargetAddress8 { Id = 2, Key1 = "3", City = "shanghai" };
            var target = new TargetUser8()
            {
                Address = new List<TargetAddress8>()
                {
                    targetAddress1,
                    targetAddress2
                }
            };
            user.To(target);
            targetAddress1.Should().BeEquivalentTo(new TargetAddress8 { Id = 1, Key1 = "1", City = "beijing" });
            targetAddress2.Should().BeEquivalentTo(new TargetAddress8 { Id = 3, Key1 = "3", City = "wuhan" });
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
        public class User5
        {
            public Address5 Address { get; set; }
        }
        public class Address5
        {
            public string City { get; set; }
        }
        public class TargetUser5
        {
            public string AddressCity { get; set; }

        }

        public class User6
        {
            public Address6[] Address { get; set; }
        }
        public class Address6
        {
            public int Id { get; set; }
            public string City { get; set; }
        }
        public class TargetUser6
        {
            public List<TargetAddress6> Address { get; set; }

        }

        public class TargetAddress6
        {
            public int Id { get; set; }
            public string City { get; set; }
        }

        public class User7
        {
            public Address7[] Address { get; set; }
        }
        public class Address7
        {
            public int Id { get; set; }
            public string Key1 { get; set; }
            public string City { get; set; }
        }
        public class TargetUser7
        {
            public List<TargetAddress7> Address { get; set; }

        }

        public class TargetAddress7
        {
            public int Id { get; set; }
            public string Key1 { get; set; }
            public string City { get; set; }
        }

        public class User8
        {
            public Address8[] Address { get; set; }
        }
        public class Address8
        {
            public int Id { get; set; }
            [Key]
            public string Key1 { get; set; }
            public string City { get; set; }
        }
        public class TargetUser8
        {
            public List<TargetAddress8> Address { get; set; }

        }

        public class TargetAddress8
        {
            public int Id { get; set; }
            public string Key1 { get; set; }
            public string City { get; set; }
        }

    }
}
