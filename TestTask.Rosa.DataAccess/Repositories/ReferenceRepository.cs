using Microsoft.EntityFrameworkCore;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Interfaces.Repositories;
using TestTask.Rosa.Core.Models;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess.Repositories
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly RosaDbContext _context;

        public ReferenceRepository(RosaDbContext context)
        {
            _context = context;
        }

        public async Task Create(Reference reference)
        {
            var referenceEntity = new ReferenceEntity()
            {
                Id = reference.Id,
                UserId = reference.EmployeeId,
                Type = reference.Type,
                CopiesCount = reference.CopiesCount,
                Reason = reference.Reason,
                Status = reference.Status,
                CreatedAt = reference.CreatedAt,
                UpdatedAt = reference.UpdatedAt,
                ClosedAt = reference.ClosedAt
            };

            await _context.References.AddAsync(referenceEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Reference?> GetById(Guid id)
        {
            var reference = await _context
                .References
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reference is null)
            {
                return null;
            }

            return MapToDomain(reference);
        }

        public async Task<List<Reference>> GetByEmployeeId(Guid userId)
        {
            var referenceEntity = await _context
                .References
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var references = referenceEntity
                 .Select(MapToDomain)
                 .ToList();

            return references;
        }

        public async Task<List<Reference>> GetAll()
        {
            var referenceEntity = await _context
                .References
                .AsNoTracking()
                .ToListAsync();

            var references = referenceEntity
                 .Select(MapToDomain)
                 .ToList();

            return references;
        }

        public async Task<bool> Update(Reference reference)
        {
            var affectedRows = await _context
                .References
                .Where(x => x.Id == reference.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Status, reference.Status)
                    .SetProperty(r => r.UpdatedAt, reference.UpdatedAt)
                    .SetProperty(r => r.ClosedAt, reference.ClosedAt));

            return affectedRows > 0;
        }

        public async Task<bool> ExistsActive(Guid employeeId, ReferenceType type)
        {
            return await _context
                .References
                .AsNoTracking()
                .AnyAsync(x =>
                    x.UserId == employeeId &&
                    x.Type == type &&
                    x.Status != ReferenceStatus.Closed);
        }

        private static Reference MapToDomain(ReferenceEntity referenceEntity)
        {
            var result = Reference.Create(
                referenceEntity.Id,
                referenceEntity.UserId,
                referenceEntity.Type,
                referenceEntity.CopiesCount,
                referenceEntity.Reason,
                referenceEntity.Status,
                referenceEntity.CreatedAt,
                referenceEntity.UpdatedAt,
                referenceEntity.ClosedAt);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
