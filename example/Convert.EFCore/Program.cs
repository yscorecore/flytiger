// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using Convert.EFCore;

await using var context = new TestDbContext();
await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();
context.Users.Add(new User()
{
    Id = Guid.NewGuid(),
    Name = "zhangsan",
    Address = new List<Address> { new Address() { Id = Guid.NewGuid(), City = "xi'an" } },
    Age = 18,
    Roles = new List<Role> { new Role() { Name = "admin", Id = Guid.NewGuid() } }
});
await context.SaveChangesAsync();
var userdtos = context.Users.To<UserDto>().ToList();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(userdtos));
