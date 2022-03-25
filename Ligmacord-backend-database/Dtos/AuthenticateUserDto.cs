namespace Ligmacord_backend_database.Dtos;

public record AuthenticateUserDto
{
    public string UserName { set; get; }
    public string Password { set; get; }
}