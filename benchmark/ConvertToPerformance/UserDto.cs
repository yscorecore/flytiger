namespace ConvertToPerformance;

public class UserDto
{
    public string Name { get; set; }

    public DateTime Birthday { get; set; }

    public AddressDto[] Addresses { get; set; }

    public RoleDto Role { get; set; }
}

public class RoleDto
{
    public string Name { get; set; }


}
public class AddressDto
{
    public string Province { get; set; }
    public string City { get; set; }

    public string Street { get; set; }

    public string Tel { get; set; }

}
