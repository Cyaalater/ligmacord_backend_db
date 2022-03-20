using System.ComponentModel.DataAnnotations;

namespace Ligmacord_backend_database.Dtos;

public record FriendAddDto()
{
    [Required]
    public Guid OwnId { get; init; }
    [Required]
    public Guid FriendId { get; init; }
}