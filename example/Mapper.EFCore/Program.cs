using System.Text.Json;

namespace Mapper.EFCore
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = await CreateContext();
            var userService = new UserService(context);
            await userService.AddUser(new UserDto()
            {
                Id = Guid.NewGuid(),
                Name = "zhangsan",
                Address = new AddressDto[] { new AddressDto() { Id = Guid.NewGuid(), City = "xi'an" } },
                Age = 18,
                Roles = new List<RoleDto> { new RoleDto() { Name = "admin", Id = Guid.NewGuid() } }
            });

            var userDtos = await userService.ListAll();
            Console.WriteLine(JsonSerializer.Serialize(userDtos));
        }
        static async Task<TestDbContext> CreateContext()
        {
            var context = new TestDbContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return context;
        }
    }

}
