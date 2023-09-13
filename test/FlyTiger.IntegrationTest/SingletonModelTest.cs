using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
