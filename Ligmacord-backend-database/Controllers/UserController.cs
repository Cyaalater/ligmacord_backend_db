using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using Ligmacord_backend_database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligmacord_backend_database.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }
    
    // User must authenticate before doing any action
    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public Tokens AuthenticateUser(AuthenticateUserDto _authenticateUser)
    {
        return _userRepository.Authenticate(_authenticateUser);
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> UsersGet()
    {
        // Debug test for how to identify user from the token
        Console.WriteLine(User.Identity.Name);
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
            Id = Guid.NewGuid(),
            Username = createUserDto.username,
            Email = createUserDto.email,
            Password = createUserDto.password.GenerateHash(),
            Channels = new List<Guid>(),
            Friends = new List<Guid>(),
            DateCreated = DateTimeOffset.Now
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
        List<Guid> friendsList = ownUser.Friends;
        for (int i = 0; i < friendsList.Count; i++)
        {
            yield return (await _userRepository.GetUserAsync(friendsList[i])).asDto();
        }
        

    }
}