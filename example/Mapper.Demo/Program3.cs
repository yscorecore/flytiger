//using FlyTiger;
//namespace Mapper.Demo
//{
//    [Mapper(typeof(UserEntity), typeof(UserDto))]
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            var user = new UserEntity { Age = 10, Name = "zhangsan", City = new CityEntity { Name = "xi'an", Province = "shannxi" } };
//            var userDto = user.To<UserDto>();
//            Console.WriteLine(userDto);
//        }
//    }
//    public record UserEntity
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public int Age { get; set; }
//        public CityEntity City { get; set; }
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
//        public Guid CityId { get; set; }
//        public string CityName { get; set; }
//        public string CityProvince { get; set; }
//    }
   

//}
