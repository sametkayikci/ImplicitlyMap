namespace ImplicitlyMap.Example.Dtos;
public class MemberDto
{
    public MemberDto(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; set; }
    public string Password { get; set; }

   
}