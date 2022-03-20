using System.ComponentModel.DataAnnotations;

namespace Ligmacord_backend_database.Dtos;

public record CreateUserDto
{
    [Required]
    public string username { get; init; }
    [Required]
    public string email { get; init; }
    [Required]
    public string password { get; init; }
}