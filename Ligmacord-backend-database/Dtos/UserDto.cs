namespace Ligmacord_backend_database.Dtos;

public record UserDto
{
    public Guid id { get; init; }
    public string username { get; init; }
    public DateTimeOffset dateCreated { get; init; }
}