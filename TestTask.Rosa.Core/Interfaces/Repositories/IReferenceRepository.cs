using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Core.Interfaces.Repositories
{
    public interface IReferenceRepository
    {
        Task Create(Reference reference);
        Task<bool> ExistsActive(Guid employeeId, ReferenceType type);
        Task<List<Reference>> GetAll();
        Task<List<Reference>> GetByEmployeeId(Guid userId);
        Task<Reference?> GetById(Guid id);
        Task<bool> Update(Reference reference);
    }
}