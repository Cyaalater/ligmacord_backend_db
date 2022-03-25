namespace Ligmacord_backend_database.Entities;

public record Channel
{
    public Guid Id { get; init; }
    
    public string Title { get; init; }
    
    public Guid OwnerId { get; set; }
    public List<Guid> UsersId { get; init; }
    
    public List<Message> Messages {get; init; }
}