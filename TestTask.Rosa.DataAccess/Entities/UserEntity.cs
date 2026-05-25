using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.DataAccess.Entities
{
    /// <summary>
    /// Сущность пользователя для хранения в базе данных.
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// Заявки на справки, созданные пользователем.
        /// </summary>
        public ICollection<ReferenceEntity> References { get; set; } = new List<ReferenceEntity>();
    }
}
