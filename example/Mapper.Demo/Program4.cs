//using FlyTiger;
//namespace Mapper.Demo
//{
//    [Mapper(typeof(UserEntity), typeof(UserDto))]
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            var user = new UserEntity { Age = 10, Name = "zhangsan", Cities =  new List<CityEntity> { new CityEntity { Name = "xi'an", Province = "shannxi" } } };
//            var userDto = user.To<UserDto>();
//            Console.WriteLine(userDto);
//        }
//    }
//    public record UserEntity
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public int Age { get; set; }
//        public List<CityEntity> Cities { get; set; }
//    }
//    public record CityEntity
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Province { get; set; }
//    }
//    public record UserDto
//    {
//        public string Name { get; set; }
//        public int Age { get; set; }
//        public CityDto[] Cities { get; set; }
//    }
//    public record CityDto
//    {
//        public string Name { get; set; }
//        public string Province { get; set; }
//    }

//}
