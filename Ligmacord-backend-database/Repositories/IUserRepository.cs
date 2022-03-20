using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserAsync(Guid id);
    public Task<IEnumerable<User>> GetUsersAsync();

    public Task AddFriendAsync(Guid id, Guid friend_id);

    public Task RemoveFriendAsync(Guid id, Guid friend_id);
    public Task AddUserAsync(User user);
}