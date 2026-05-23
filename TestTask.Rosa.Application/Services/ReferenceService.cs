using CSharpFunctionalExtensions;
using TestTask.Rosa.Application.Interfaces.Services;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Interfaces.Repositories;
using TestTask.Rosa.Core.Models;

namespace TestTask.Rosa.Application.Services
{
    public class ReferenceService : IReferenceService
    {
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceService(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public async Task<Result<Guid>> CreateReference(Guid employeeId, ReferenceType type, int copiesCount, string reason)
        {
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<Guid>($"{nameof(employeeId)} не может быть пустым");
            }

            if (await _referenceRepository.ExistsActive(employeeId, type))
            {
                return Result.Failure<Guid>($"Справка такого типа уже в работе");
            }

            var referenceResult = Reference.Create(
                Guid.NewGuid(),
                employeeId,
                type,
                copiesCount,
                reason);

            if (referenceResult.IsFailure)
            {
                return Result.Failure<Guid>(referenceResult.Error);
            }

            await _referenceRepository.Create(referenceResult.Value);
            return Result.Success(referenceResult.Value.Id);
        }

        public async Task<Result<Reference>> GetReferenceById(Guid id)
        {
            var reference = await _referenceRepository.GetById(id);

            if (reference is null)
            {
                return Result.Failure<Reference>("Справка не найдена");
            }

            return Result.Success(reference);
        }

        public async Task<Result<List<Reference>>> GetAllReferencesByEmployeeId(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<List<Reference>>($"{nameof(employeeId)} не найден");
            }

            var references = await _referenceRepository.GetByEmployeeId(employeeId);

            return Result.Success(references);
        }

        public async Task<Result<List<Reference>>> GetAllReferences()
        {
            var references = await _referenceRepository.GetAll();

            return Result.Success(references);
        }

        public async Task<Result> UpdateReferenceStatus(Guid referenceId, ReferenceStatus status)
        {
            if (referenceId == Guid.Empty)
            {
                return Result.Failure($"{nameof(referenceId)} не может быть пустым");
            }

            var reference = await _referenceRepository.GetById(referenceId);

            if (reference is null)
            {
                return Result.Failure($"{nameof(reference)} не найден");
            }

            var updateResult = reference.UpdateStatus(status);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            var isUpdated = await _referenceRepository.Update(reference);

            if (!isUpdated)
            {
                return Result.Failure("Не удалось обновить справку");
            }

            return Result.Success();
        }
    }
}
