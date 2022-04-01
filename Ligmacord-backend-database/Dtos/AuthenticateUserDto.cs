using System.ComponentModel.DataAnnotations;

namespace Ligmacord_backend_database.Dtos;

public record AuthenticateUserDto
{
    [Required]
    public string UserName { set; get; }
    [Required]
    public string Password { set; get; }
}