---
layout: default
title: ConvertTo
nav_order: 3
permalink: /convert-to
parent: FEATURES
---

## ConvertTo

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