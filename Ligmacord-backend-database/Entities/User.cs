namespace Ligmacord_backend_database.Entities;

public record User
{
    public Guid id { get; init; }
    public string username { get; init; }
    public string email { get; init; }
    public string password { get; init; }
    
    public List<Guid> friends { get; init; }
    public DateTimeOffset dateCreated { get; init; }
}