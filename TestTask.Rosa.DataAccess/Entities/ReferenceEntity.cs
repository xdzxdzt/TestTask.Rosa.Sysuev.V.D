using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.DataAccess.Entities
{
    /// <summary>
    /// Сущность заявки на справку для хранения в базе данных.
    /// </summary>
    public class ReferenceEntity
    {
        /// <summary>
        /// Идентификатор заявки.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, создавшего заявку.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Пользователь, создавший заявку.
        /// </summary>
        public UserEntity User { get; set; } = null!;

        /// <summary>
        /// Тип запрашиваемой справки.
        /// </summary>
        public ReferenceType Type { get; set; }

        /// <summary>
        /// Количество экземпляров справки.
        /// </summary>
        public int CopiesCount { get; set; } = 0;

        /// <summary>
        /// Причина получения справки.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Статус заявки.
        /// </summary>
        public ReferenceStatus Status { get; set; }

        /// <summary>
        /// Дата и время создания заявки.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата и время последнего обновления заявки.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Дата и время закрытия заявки.
        /// </summary>
        public DateTime? ClosedAt { get; set; }
    }
}
