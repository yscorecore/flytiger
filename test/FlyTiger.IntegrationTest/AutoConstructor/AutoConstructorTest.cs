﻿using System.Diagnostics;
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
            var c4 = new Class4("field2value");
            c4.Field.Should().Be("frominit");
            c4.Field2.Should().Be("field2value");
        }
        [Fact]
        public void ShouldInvokeInitializeMethodWhenEmptyCtorGenerated()
        {
            var c5 = new Class5();
            Class5.count.Should().Be(1);
        }

        [Fact]
        public void ShouldGenerateArgumentsFromBaseClass()
        {
            var c6 = new Class6(1, "prop", 2);
            c6.AutoProp1.Should().Be("prop");
            c6.Field2.Should().Be(2);
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
            public string Field2 { get; set; }
            public string Field { get => field; set => field = value; }
            [AutoConstructorInitialize]
            public void Init()
            {
                this.field = "frominit";
            }
        }
        [AutoConstructor]
        partial class Class5
        {
            public static int count;
            [AutoConstructorInitialize]
            public void Init()
            {
                count++;
            }
        }
        [AutoConstructor]
        partial class Class6Base
        {
            public string AutoProp1 { get; set; }
            private readonly int field1;
        }
        [AutoConstructor]
        partial class Class6 : Class6Base
        {
            private readonly int field2;

            public int Field2 => field2;
        }


    }
}
