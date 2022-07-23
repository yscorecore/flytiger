---
layout: default
title: SingletonPattern
nav_order: 4
parent: FEATURES
---

## SingletonPattern

`FlyTiger.SingletonPatternAttribute` will help us define a singleton pattern.

- No Use FlyTiger
    ```csharp
    public class Service1
    {
        private static readonly Lazy<Service1> LazyInstance = new Lazy<Service1>(() => new Service1(), true);

        public static Service1 Instance => LazyInstance.Value;

        private class Service1() 
        {
        }

        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Service1.Instance.SayHello("FlyTiger");
        }
    }
    ```
- Use FlyTiger
    ```csharp
    [SingletonPattern]
    public partial class Service1
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Service1.Instance.SayHello("FlyTiger");
        }
    }
    ```
### ConvertTo
`FlyTiger.SingletonPatternAttribute` will help us generate mapping code in different models.

- No Use FlyTiger
    ```csharp
    [AutoConstructor]
    class UserService
    {
        private readonly UserDbContext userContext;
        public List<UserDtos> ListAllUsers()
        {
            return userContexts.Users.Select(p => new UserDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Tel = p.Tel,
                    Roles = p.Roles.Select(r => new RoleDto {Id = r.Id, Name = r.Name}).ToList()
                }).ToList();
        }
    }
    ```
- Use FlyTiger
    ```csharp
    [AutoConstructor]
    class UserService
    {
        private readonly UserDbContext userContext;
        public List<UserDtos> ListAllUsers()
        {
            return userContexts.Users.To<UserDto>().ToList();
        }
    }

    [ConvertTo(typeof(User), typeof(UserDto))]
    class Mappings
    {
    }
    ```