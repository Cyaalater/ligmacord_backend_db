namespace Ligmacord_backend_database.Entities;

public record User
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    
    public List<Guid> Friends { get; init; }
    
    public List<Guid> Channels { get; init; }
    public DateTimeOffset DateCreated { get; init; }
}