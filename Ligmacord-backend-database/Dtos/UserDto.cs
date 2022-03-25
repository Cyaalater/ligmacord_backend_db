namespace Ligmacord_backend_database.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public DateTimeOffset DateCreated { get; init; }
}