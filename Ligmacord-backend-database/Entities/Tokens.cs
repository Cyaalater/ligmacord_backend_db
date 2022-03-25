namespace Ligmacord_backend_database.Entities;

public record Tokens
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}