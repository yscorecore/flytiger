using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyTiger;
#pragma warning disable CS0169 
namespace AutoCtor
{
    [AutoConstructor]
    public partial class Demo1
    {
        private string strValue;
        private int intValue;
    }
    [AutoConstructor]
    public partial class Demo2
    {
        public string StrValue { get; set; }
        public int IntValue { get; set; }
    }
    [AutoConstructor(NullCheck = true)]
    public partial class Demo3
    {
        private string strValue;
        private int intValue;
    }

    [AutoConstructor]
    public partial class Demo4
    {
        private string strValue;
        [AutoConstructorIgnore]
        private int intValue;
    }

    [AutoConstructor]
    public partial class Demo5
    {
        private string strValue;
        private int intValue;

        [AutoConstructorInitialize]
        private void MyInitLogic()
        {
            Console.WriteLine("instance is creating.");
        }
    }
    public interface IService1
    {
        void Action1();
    }
    public interface IService2
    {
        void Action2();
    }
    [AutoConstructor]
    public partial class Demo6Service
    {
        private readonly IService1 service1;
        private readonly IService2 service2;
        public void Action()
        {
            service1.Action1();
            service2.Action2();
        }
    }


    [AutoConstructor]
    public partial class Demo7Parent
    {
        private string strValue;
    }
    [AutoConstructor]
    public partial class Demo7Child : Demo7Parent
    {
        private int intValue;
    }
}
