using FlyTiger;
namespace Mapper.Demo
{
    [Mapper(typeof(UserEntity), typeof(UserDto))]
    internal class Program2
    {
        static void Main(string[] args)
        {
            var userList = new List<UserEntity>
            {
                 new UserEntity { Age = 10, Name = "zhangsan", Id = Guid.NewGuid() }
            };
            var userDtoArray = userList.To<UserDto>().ToArray();
            Console.WriteLine(userDtoArray);
        }
    }
    public record UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public record UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
