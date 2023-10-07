using System.Reflection;
using FluentAssertions;

namespace FlyTiger.IntegrationTest
{
    public partial class SingletonModelTest
    {
        [Fact]
        public void ShouldGenerateSingletonModel()
        {
            Class1.Instance.Id.Should().Be(2);
        }

        [Fact]
        public void ShouldGenerateWithCustomName()
        {
            Class2.Default.Id.Should().Be(2);
        }

        [Fact]
        public void ShouldGeneratePrivateCtor()
        {
            typeof(Class1).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).First().IsPublic.Should().BeFalse();
        }

        [SingletonPattern]
        partial class Class1
        {
            public int Id { get; set; } = 2;
        }
        [SingletonPattern(InstancePropertyName = "Default")]
        partial class Class2
        {
            public int Id { get; set; } = 2;
        }

    }
}
