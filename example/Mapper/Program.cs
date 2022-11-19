using System.Text.Json;
using FlyTiger;
namespace Mapper
{
    [Mapper(typeof(From), typeof(To))]
    internal class Program
    {
        static void Main(string[] args)
        {
            var from = new From { StrProp = "hello", IntProp = 2 };
            var to = from.To<To>();
            Console.WriteLine(JsonSerializer.Serialize(to));
        }
    }
}
