using System.ComponentModel.DataAnnotations;

namespace Ligmacord_backend_database.Dtos;

public record CreateMessage
{
    [Required]
    public Guid UserId { get; init; }
    public string Message { get; init; }
}