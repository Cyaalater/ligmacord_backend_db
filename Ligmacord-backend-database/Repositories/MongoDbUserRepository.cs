using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ligmacord_backend_database.Dtos;
using Ligmacord_backend_database.Entities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ligmacord_backend_database.Repositories;

public class MongoDbUserRepository : IUserRepository
{
    private const string DatabaseName = "ligmacord_db";
    private const string CollectionName = "users";
    private readonly IMongoCollection<User> _userCollection;
    private readonly FilterDefinitionBuilder<User> _filterBuilder = new FilterDefinitionBuilder<User>();
    private readonly IConfiguration _configuration; 
    
    public MongoDbUserRepository(IMongoClient mongoClient, IConfiguration _configuration)
    {
        this._configuration = _configuration;
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
        _userCollection = database.GetCollection<User>(CollectionName);
    }

    public Tokens Authenticate(AuthenticateUserDto _authenticateUser)
    {
        var filter = _filterBuilder.Where(user =>
            user.Password == _authenticateUser.Password.GenerateHash() && user.Username == _authenticateUser.UserName);
        var foundUser = _userCollection.Find(filter).SingleOrDefault();
        
        // Finish the function upon none users have same password and same username
        if (foundUser == null)
        {
            return null;
        }
        
        // Jwt creation
        var tokenHandler = new JwtSecurityTokenHandler();
        // Get key from configuration
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        // Body of the jwt
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Identity Claim is what data the token stores to identify the user
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, _authenticateUser.UserName)
            }),
            // Later expiration time will be changed to the conf
            Expires = DateTime.Now.AddMinutes(10),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Tokens() {Token = tokenHandler.WriteToken(token)};
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