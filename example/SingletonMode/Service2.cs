using System;
using FlyTiger;

namespace SingletonMode
{
    [SingletonPattern(InstancePropertyName = "Default")]
    public sealed partial class Service2
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello,{name}");
        }
    }

}
