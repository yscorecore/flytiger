//using FlyTiger;
//namespace Mapper.Demo
//{
//    [Mapper(typeof(UserEntity), typeof(UserDto))]
//    internal class Program2
//    {
//        static void Main(string[] args)
//        {
//            var user = new UserEntity { Age = 10, Name = "zhangsan", Id = Guid.NewGuid() };
//            var userDto = user.To<UserDto>();
//            Console.WriteLine(userDto);
//        }
//    }
//    public record UserDto
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public int Age { get; set; }
//    }
//    public record UserEntity
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public int Age { get; set; }
//    }
//}
