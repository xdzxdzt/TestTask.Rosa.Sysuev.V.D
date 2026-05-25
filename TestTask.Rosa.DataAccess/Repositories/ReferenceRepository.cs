using Microsoft.EntityFrameworkCore;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Interfaces.Repositories;
using TestTask.Rosa.Core.Models;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий для работы с заявками на справки.
    /// </summary>
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly RosaDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория заявок.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public ReferenceRepository(RosaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создает новую заявку на справку.
        /// </summary>
        /// <param name="reference">Доменная модель заявки.</param>
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

        /// <summary>
        /// Получает заявку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <returns>Заявка или null, если она не найдена.</returns>
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

        /// <summary>
        /// Получает все заявки указанного сотрудника.
        /// </summary>
        /// <param name="userId">Идентификатор сотрудника.</param>
        /// <returns>Список заявок сотрудника.</returns>
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

        /// <summary>
        /// Получает все заявки на справки.
        /// </summary>
        /// <returns>Список всех заявок.</returns>
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

        /// <summary>
        /// Обновляет статус и даты заявки.
        /// </summary>
        /// <param name="reference">Доменная модель заявки с обновленными данными.</param>
        /// <returns>Значение true, если запись была обновлена.</returns>
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

        /// <summary>
        /// Проверяет наличие активной заявки указанного типа у сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="type">Тип справки.</param>
        /// <returns>Значение true, если активная заявка существует.</returns>
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

        /// <summary>
        /// Преобразует сущность базы данных в доменную модель.
        /// </summary>
        /// <param name="referenceEntity">Сущность заявки.</param>
        /// <returns>Доменная модель заявки.</returns>
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
