using Microsoft.EntityFrameworkCore;
using TestTask.Rosa.Core.Enums;
using TestTask.Rosa.Core.Interfaces.Repositories;
using TestTask.Rosa.Core.Models;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess.Repositories
{
    /// <summary>
    /// Репозиторий для чтения данных пользователей.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly RosaDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория пользователей.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public UserRepository(RosaDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверяет наличие пользователя с указанной ролью.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="role">Ожидаемая роль пользователя.</param>
        /// <returns>Значение true, если пользователь с указанной ролью существует.</returns>
        public async Task<bool> ExistsWithRole(Guid userId, UserRole role)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId && x.Role == role);
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Пользователь или null, если он не найден.</returns>
        public async Task<User?> GetById(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user is null
                ? null
                : MapToDomain(user);
        }

        /// <summary>
        /// Получает пользователей по набору идентификаторов.
        /// </summary>
        /// <param name="userIds">Идентификаторы пользователей.</param>
        /// <returns>Список найденных пользователей.</returns>
        public async Task<List<User>> GetByIds(IEnumerable<Guid> userIds)
        {
            var ids = userIds
                .Distinct()
                .ToList();

            var users = await _context.Users
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return users
                .Select(MapToDomain)
                .ToList();
        }

        /// <summary>
        /// Преобразует сущность пользователя в доменную модель.
        /// </summary>
        /// <param name="userEntity">Сущность пользователя.</param>
        /// <returns>Доменная модель пользователя.</returns>
        private static User MapToDomain(UserEntity userEntity)
        {
            var result = User.Create(
                userEntity.Id,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.Role);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
