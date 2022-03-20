using Ligmacord_backend_database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ligmacord_backend_database.Repositories;

public class MongoDbUserRepository : IUserRepository
{
    private const string DatabaseName = "ligmacord_db";
    private const string CollectionName = "users";
    private readonly IMongoCollection<User> _userCollection;
    private readonly FilterDefinitionBuilder<User> filterBuilder = new FilterDefinitionBuilder<User>();

    public MongoDbUserRepository(IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
        _userCollection = database.GetCollection<User>(CollectionName);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        var filter = filterBuilder.Where((user => user.id == id));
        return await _userCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task AddFriendAsync(Guid id, Guid friend_id)
    {
        var filter = filterBuilder.Where((user => user.id == id));
        await _userCollection.UpdateOneAsync(filter,
            Builders<User>.Update.AddToSet(userExist1 => userExist1.friends, friend_id));
    }

    public async Task RemoveFriendAsync(Guid id, Guid friend_id)
    {
        var filter = filterBuilder.Where((user => user.id == id));
        await _userCollection.UpdateOneAsync(
            filter,
            Builders<User>.Update.Pull(user => user.friends, friend_id)
        );
    }
}