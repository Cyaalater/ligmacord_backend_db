using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database.Dtos;

public record ChannelDto
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }
    public List<Guid> UsersId { get; init; }
    
    public List<Message> Messages {get; init; }
}