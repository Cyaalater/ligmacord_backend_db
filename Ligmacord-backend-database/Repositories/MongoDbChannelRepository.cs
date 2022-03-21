using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using MongoDB.Driver;

namespace Ligmacord_backend_database.Repositories;

public class MongoDbChannelRepository : IChannelRepository
{
    private const string DatabaseName = "ligmacord_db";
    private const string CollectionName = "channels";
    private readonly IMongoCollection<Channel> _channelCollection;
    private readonly FilterDefinitionBuilder<Channel> _filterBuilder = new FilterDefinitionBuilder<Channel>();

    public MongoDbChannelRepository(IMongoClient mongoClient)
    {
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
        _channelCollection = database.GetCollection<Channel>(CollectionName);
    }

    public async Task CreateChannelAsync(Channel channel)
    {
        var filter = _filterBuilder.Where((channel1 => channel1.Id == channel.Id));
        if ((await _channelCollection.FindAsync(filter)) != null)
        {
            return;
        }
        await _channelCollection.InsertOneAsync(channel);
    }

    public async Task AddToChannelAsync(Guid channelId, Guid invitedId)
    {
        var filter = _filterBuilder.Where((channel => channel.Id == channelId));
        await _channelCollection.UpdateOneAsync(filter,
            Builders<Channel>.Update.AddToSet(channel => channel.UsersId, invitedId)
        );
    }

    public async Task KickFromChannelAsync(Guid channelId, Guid kickedId)
    {
        var filter = _filterBuilder.Where((channel => channel.Id == channelId));
        await _channelCollection.UpdateOneAsync(filter,
            Builders<Channel>.Update.Pull(channel => channel.UsersId, kickedId)
        );
    }

    public async Task RemoveChannelAsync(Guid channelId)
    {
        var filter = _filterBuilder.Where((channel => channel.Id == channelId));
        await _channelCollection.DeleteOneAsync(filter);
    }

    public async Task AddMessageAsync(Guid channelId, Guid userId, string message)
    {
        var filter = _filterBuilder.Where((channel => channel.Id == channelId));
        var currentChannel = (await _channelCollection.FindAsync(filter)).SingleOrDefault();
        if (!currentChannel.UsersId.Contains(userId))
        {
            return;
        }

        await _channelCollection.UpdateOneAsync(filter,
            Builders<Channel>.Update.AddToSet(channel => channel.Messages, new Message()
            {
                OwnerId = userId,
                MessageText = message,
                DateCreated = DateTimeOffset.Now
            }));
    }

    public async Task<Channel> GetChannelAsync(Guid cid)
    {
        var filter = _filterBuilder.Where((channel => channel.Id == cid));
        return (await _channelCollection.FindAsync(filter)).SingleOrDefault();
    }
}