using Ligmacord_backend_database.Entities;
using MongoDB.Driver;

namespace Ligmacord_backend_database.Repositories;

public class MongoDbSessionRepository : ISessionRepository
{
    private const string DatabaseName = "ligmacord_db";
    private const string CollectionName = "sessions";
    private readonly IMongoCollection<Sessions> _sessionCollection;
    private readonly FilterDefinitionBuilder<Sessions> _filterBuilder = new FilterDefinitionBuilder<Sessions>();
    private readonly IConfiguration _configuration; 
    
    public MongoDbSessionRepository(IMongoClient _mongoClient, IConfiguration _configuration)
    {
        var database = _mongoClient.GetDatabase(DatabaseName);
        _sessionCollection = database.GetCollection<Sessions>(CollectionName);
        this._configuration = _configuration;

    }
    
    public async Task<Sessions> AddSession(Guid id)
    {
        var newSession = new Sessions()
        {
            ExpireDate = DateTimeOffset.Now.AddHours(2),
            SessionId = Guid.NewGuid(),
            UserId = id
        };
        await _sessionCollection.InsertOneAsync(newSession);
        return newSession;
    }

    public async Task<Guid> CheckSession(Guid sessionId)
    {
        var filter = _filterBuilder.Where((sessions => sessions.SessionId == sessionId));
        return (await _sessionCollection.FindAsync(filter)).SingleOrDefault().UserId;
    }
}