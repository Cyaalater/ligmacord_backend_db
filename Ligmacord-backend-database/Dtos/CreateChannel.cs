using System.ComponentModel.DataAnnotations;

namespace Ligmacord_backend_database.Dtos;

public record CreateChannel
{
    [Required]
    public Guid UserId { get; init; }
    [Required]
    public string Title { get; init; }
    
}