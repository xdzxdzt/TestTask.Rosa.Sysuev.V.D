using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsWithRole(Guid userId, UserRole role);
        Task<User?> GetById(Guid userId);
        Task<List<User>> GetByIds(IEnumerable<Guid> userIds);
    }
}
