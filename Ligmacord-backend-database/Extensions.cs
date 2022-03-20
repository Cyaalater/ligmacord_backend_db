using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database;

public static class Extensions
{
    public static UserDto asDto(this User user)
    {
        return new UserDto()
        {
            id = user.id,
            username = user.username,
            dateCreated = user.dateCreated
        };
    }
}