using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database.Repositories;

public interface IChannelRepository
{
    public Task CreateChannelAsync(Channel channel);
    public Task AddToChannelAsync(Guid channelId, Guid invitedId);
    public Task KickFromChannelAsync(Guid channelId, Guid kickedId);
    public Task RemoveChannelAsync(Guid channelId);

    public Task AddMessageAsync(Guid channelId, Guid userId, string message);

    public Task<Channel> GetChannelAsync(Guid cid);
}