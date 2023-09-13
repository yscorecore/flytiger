using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace FlyTiger.IntegrationTest
{
    public partial class AutoConstructorTest
    {
        [Fact]
        public void ShouldGenerateNothing()
        {
            _ = new Class1();
        }

        [Fact]
        public void ShouldGenerate1ArgCtorWithField()
        {
            var c2 = new Class2("fieldvalue");
            c2.Field.Should().Be("fieldvalue");
        }

        [Fact]
        public void ShouldIgrnoeFieldWhenDefineAutoConstructorIgnoreAttribute()
        {
            _ = new Class3();
        }
        [Fact]
        public void ShouldInvokeInitializeMethod()
        {
            var c4  = new Class4();
            c4.Field.Should().Be("frominit");
        }


        [AutoConstructor]
        partial class Class1
        { 
        
        }
        
        [AutoConstructor]
        partial class Class2
        {
            private string field;

            public string Field { get => field; set => field = value; }
        }
        [AutoConstructor]
        partial class Class3
        {
            [AutoConstructorIgnore]
            private string field;
            public string Field { get => field; set => field = value; }
        }

        [AutoConstructor]
        partial class Class4
        {
            [AutoConstructorIgnore]
            private string field;
            public string Field { get => field; set => field = value; }
            [AutoConstructorInitialize]
            public void Init()
            {
                this.field = "frominit";
            }
        }
    }
}
