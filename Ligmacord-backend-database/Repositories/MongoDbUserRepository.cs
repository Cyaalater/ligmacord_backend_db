using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ligmacord_backend_database.Repositories;

public class MongoDbUserRepository : IUserRepository
{
    private const string DatabaseName = "ligmacord_db";
    private const string CollectionName = "users";
    private readonly IMongoCollection<User> _userCollection;
    private readonly FilterDefinitionBuilder<User> _filterBuilder = new FilterDefinitionBuilder<User>();

    public MongoDbUserRepository(IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
        _userCollection = database.GetCollection<User>(CollectionName);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        var filter = _filterBuilder.Where((user => user.Id == id));
        return await _userCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async IAsyncEnumerable<Guid> GetUserChannelsAsync(Guid id)
    {
        var filter = _filterBuilder.Where((user => user.Id == id));
        var channels = (await _userCollection.FindAsync(filter)).SingleOrDefault().Channels;
        for (int i = 0; i < channels.Count; i++)
        {
            yield return channels[i];

        }
    }

    public async Task AddUserAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task AddFriendAsync(Guid id, Guid friend_id)
    {
        var filter = _filterBuilder.Where((user => user.Id == id));
        await _userCollection.UpdateOneAsync(filter,
            Builders<User>.Update.AddToSet(userExist1 => userExist1.Friends, friend_id));
    }

    public async Task RemoveFriendAsync(Guid id, Guid friend_id)
    {
        var filter = _filterBuilder.Where((user => user.Id == id));
        await _userCollection.UpdateOneAsync(
            filter,
            Builders<User>.Update.Pull(user => user.Friends, friend_id)
        );
    }
}