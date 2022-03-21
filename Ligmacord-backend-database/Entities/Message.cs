namespace Ligmacord_backend_database.Entities;

public record Message
{
    public Guid OwnerId { get; init; }
    
    public string MessageText { get; init; }
    
    public DateTimeOffset DateCreated { get; init; }
}