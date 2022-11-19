using System;
using System.Collections.Generic;

namespace Mapper.EFCore
{
    public class BaseDto
    {
        public Guid Id { get; set; }
    }

    public class UserDto : BaseDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public AddressDto[] Address { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
    public class SampleUserDto : BaseDto
    {
        public string Name { get; set; }
    }

    public class RoleDto : BaseDto
    {
        public string Name { get; set; }
    }

    public class AddressDto : BaseDto
    {
        public string City { get; set; }
    }
}
