namespace ConvertToPerformance;

public class UserInfo
{
    public string Name { get; set; }

    public DateTime Birthday { get; set; }

    public List<AddressInfo> Addresses { get; set; }

    public RoleInfo Role { get; set; }
}

public class RoleInfo
{
    public string Name { get; set; }


}
public class AddressInfo
{
    public string Province { get; set; }
    public string City { get; set; }

    public string Street { get; set; }

    public string Tel { get; set; }

}

