using FlyTiger;
using Microsoft.EntityFrameworkCore;

namespace Mapper.EFCore
{
    [AutoConstructor]
    [Mapper(typeof(UserDto), typeof(User))]
    [Mapper(typeof(User), typeof(SampleUserDto))]
    public partial class UserService
    {
        private readonly TestDbContext _context;
        public async Task AddUser(UserDto userDto)
        {
            _context.Users.Add(userDto.To<User>());
            await _context.SaveChangesAsync();
        }
        public Task<List<SampleUserDto>> ListAll(int limit = 10, int offset = 0)
        {
            return _context.Users.To<SampleUserDto>().Skip(offset).Take(limit).ToListAsync();
        }
    }
}
