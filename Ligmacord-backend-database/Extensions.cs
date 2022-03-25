using System.Security.Cryptography;
using System.Text;
using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database;

public static class Extensions
{
    public static UserDto asDto(this User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Username = user.Username,
            DateCreated = user.DateCreated
        };
    }

    public static ChannelDto asDto(this Channel channel)
    {
        return new ChannelDto()
        {
            Id = channel.Id,
            Messages = channel.Messages,
            Title = channel.Title,
            UsersId = channel.UsersId
        };
    }
    
    public static bool AuthenticatePassword(this User user, string inputPassword)
    {
        var hash = inputPassword.GenerateHash();
        return hash == user.Password;
    }

    public static string GenerateHash(this string password)
    {
        byte[] bytePassword = Encoding.Unicode.GetBytes(password);
        MD5 encryptor = MD5.Create();
        var result = encryptor.ComputeHash(bytePassword);
        Console.WriteLine(Convert.ToBase64String(result));
        return Convert.ToBase64String(result);
    }
    

    public static bool ContainsUser(this Channel channel, Guid id)
    {
        return (channel.UsersId.Contains(id));
    }
}