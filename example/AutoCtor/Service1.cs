using System;
using FlyTiger;

namespace AutoCtor
{
    [AutoConstructor(NullCheck = true)]
    public partial class Service1
    {
        private readonly Service2 _service2;
        private readonly string _value;

        public void Run()
        {
            Console.WriteLine(_service2.Convert(_value));
        }
    }


    [AutoConstructor]
    public partial class Service2
    {
        private readonly Func<string, string> _func;

        public string? Convert(string input)
        {
            return _func?.Invoke(input);
        }
    }
}
