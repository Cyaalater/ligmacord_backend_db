using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using Ligmacord_backend_database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ligmacord_backend_database.Controllers;

[ApiController]
[Route("/channels")]
public class ChannelController
{
    private IChannelRepository _channelRepository;
    public ChannelController(IChannelRepository channelRepository)
    {
        this._channelRepository = channelRepository;
    }

    [HttpPost]
    public async Task<Channel> CreateChannel(CreateChannel createChannel)
    {
        List<Guid> usersList = new List<Guid>();
        usersList.Add(createChannel.UserId);
        var newChannel = new Channel()
        {
            Id = Guid.NewGuid(),
            Messages = new List<Message>(),
            Title = createChannel.Title,
            UsersId = usersList
        };
        await _channelRepository.CreateChannelAsync(newChannel);
        return newChannel;
        // TODO: Change to dto
    }

    [HttpDelete("{cid}")]
    public async Task RemoveChannel(Guid cid)
    {
        await _channelRepository.RemoveChannelAsync(cid);
    }

    [HttpPut("{cid}/{userId}")]
    public async Task InviteChannel(Guid cid, Guid userId)
    {
        await _channelRepository.AddToChannelAsync(cid, userId);
    }

    [HttpDelete("{cid}/{userId}")]
    public async Task KickChannel(Guid cid, Guid userId)
    {
        if ((await _channelRepository.GetChannelAsync(cid)).ContainsUser(userId))
            await _channelRepository.KickFromChannelAsync(cid, userId);
    }

    [HttpPut("{cid}")]
    public async Task MessageChannel(Guid cid,CreateMessage createMessage)
    {
        if ((await _channelRepository.GetChannelAsync(cid)).ContainsUser(cid))
            await _channelRepository.AddMessageAsync(cid, createMessage.UserId, createMessage.Message);
    }

    [HttpGet("{cid}")]
    public async Task<ChannelDto> GetChannel(Guid cid)
    {
        return (await _channelRepository.GetChannelAsync(cid)).asDto();
    }
    
    
}