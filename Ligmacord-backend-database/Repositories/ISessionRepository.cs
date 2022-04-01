using Ligmacord_backend_database.Entities;

namespace Ligmacord_backend_database.Repositories;

public interface ISessionRepository
{
    Task<Sessions> AddSession(Guid id);

    Task<Guid> CheckSession(Guid sessionId);
    
    
}