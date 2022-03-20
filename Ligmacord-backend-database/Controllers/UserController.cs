using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using Ligmacord_backend_database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ligmacord_backend_database.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> UsersGet()
    {
        var users = (await _userRepository.GetUsersAsync()).Select(user => user.asDto());
        return users;
    }
    
    [HttpGet("get/{pid}")]
    public async Task<ActionResult<UserDto>> UserGet(Guid pid)
    {
        var user = await _userRepository.GetUserAsync(pid);
        if (user is null)
        {
            return NotFound();
        }
        return user.asDto();
    }


    [HttpPost("add")]
    public async Task<ActionResult<UserDto>> UserAdd(CreateUserDto createUserDto)
    {
        var user = new User()
        {
            id = Guid.NewGuid(),
            username = createUserDto.username,
            email = createUserDto.email,
            password = createUserDto.password,
            friends = new List<Guid>(),
            dateCreated = DateTimeOffset.Now
        };
        await _userRepository.AddUserAsync(user);
        return user.asDto();
    }

    [HttpPost("friends")]
    public async Task AddFriend(FriendAddDto friendAddDto)
    {
        await _userRepository.AddFriendAsync(friendAddDto.OwnId, friendAddDto.FriendId);
    }
    
    [HttpDelete("friends")]
    public async Task RemoveFriend(FriendAddDto friendAddDto)
    {
        await _userRepository.RemoveFriendAsync(friendAddDto.OwnId, friendAddDto.FriendId);
    }

    [HttpGet("friends/{user_id}")]
    public async IAsyncEnumerable<UserDto> GetFriends(Guid user_id)
    {
        User ownUser = await _userRepository.GetUserAsync(user_id);
        List<Guid> friendsList = ownUser.friends;
        for (int i = 0; i < friendsList.Count; i++)
        {
            yield return (await _userRepository.GetUserAsync(friendsList[i])).asDto();
        }
        

    }
}