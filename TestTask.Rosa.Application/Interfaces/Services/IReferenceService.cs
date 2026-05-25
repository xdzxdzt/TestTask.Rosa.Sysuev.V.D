using CSharpFunctionalExtensions;
using TestTask.Rosa.Application.Models;
using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.Application.Interfaces.Services;

public interface IReferenceService
{
    Task<Result<Guid>> CreateReference(Guid employeeId, ReferenceType type, int copiesCount, string reason);
    Task<Result<List<ReferenceDetails>>> GetAllReferences();
    Task<Result<List<ReferenceDetails>>> GetAllReferencesByEmployeeId(Guid employeeId);
    Task<Result<ReferenceDetails>> GetReferenceById(Guid id);
    Task<Result> UpdateReferenceStatus(Guid accountantId, Guid referenceId, ReferenceStatus status);
}
