using CSharpFunctionalExtensions;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Application.Interfaces.Services;

public interface IReferenceService
{
    Task<Result<Guid>> CreateReference(Guid employeeId, ReferenceType type, int copiesCount, string reason);
    Task<Result<List<Reference>>> GetAllReferences();
    Task<Result<List<Reference>>> GetAllReferencesByEmployeeId(Guid employeeId);
    Task<Result<Reference>> GetReferenceById(Guid id);
    Task<Result> UpdateReferenceStatus(Guid referenceId, ReferenceStatus status);
}