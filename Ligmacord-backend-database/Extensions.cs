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
            id = user.Id,
            username = user.Username,
            dateCreated = user.DateCreated
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

    public static string GenerateHash(this string password)
    {
        MD5 encryptor = MD5.Create();
        byte[] bytePassword = Encoding.Unicode.GetBytes(password); 
        return MD5.HashData(bytePassword).ToString();
    }

    public static bool ContainsUser(this Channel channel, Guid id)
    {
        return (channel.UsersId.Contains(id));
    }
}