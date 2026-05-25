using CSharpFunctionalExtensions;
using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.Core.Models
{
    /// <summary>
    /// Представляет заявку сотрудника на получение справки.
    /// </summary>
    public class Reference
    {
        /// <summary>
        /// Максимальная допустимая длина причины получения справки.
        /// </summary>
        public const int REASON_MAX_LENGTH = 5000;

        private Reference(
            Guid id,
            Guid employeeId,
            ReferenceType type,
            int copiesCount,
            string reason,
            ReferenceStatus status,
            DateTime createdAt,
            DateTime? updatedAt,
            DateTime? closedAt)
        {
            Id = id;
            EmployeeId = employeeId;
            Type = type;
            CopiesCount = copiesCount;
            Reason = reason;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ClosedAt = closedAt;
        }

        /// <summary>
        /// Идентификатор заявки.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Идентификатор сотрудника, создавшего заявку.
        /// </summary>
        public Guid EmployeeId { get; private set; }

        /// <summary>
        /// Тип запрашиваемой справки.
        /// </summary>
        public ReferenceType Type { get; private set; }

        /// <summary>
        /// Количество экземпляров справки.
        /// </summary>
        public int CopiesCount { get; private set; }

        /// <summary>
        /// Причина получения справки.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Текущий статус заявки.
        /// </summary>
        public ReferenceStatus Status { get; private set; }

        /// <summary>
        /// Дата и время создания заявки.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Дата и время последнего обновления заявки.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Дата и время закрытия заявки.
        /// </summary>
        public DateTime? ClosedAt { get; private set; }

        /// <summary>
        /// Создает заявку на справку после проверки бизнес-правил.
        /// </summary>
        /// <param name="id">Идентификатор заявки.</param>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="type">Тип справки.</param>
        /// <param name="copiesCount">Количество экземпляров.</param>
        /// <param name="reason">Причина получения справки.</param>
        /// <param name="status">Статус заявки.</param>
        /// <param name="createdAt">Дата создания заявки.</param>
        /// <param name="updatedAt">Дата последнего обновления заявки.</param>
        /// <param name="closedAt">Дата закрытия заявки.</param>
        /// <returns>Результат с созданной заявкой или текстом ошибки.</returns>
        public static Result<Reference> Create(
            Guid id,
            Guid employeeId,
            ReferenceType type,
            int copiesCount,
            string reason,
            ReferenceStatus status = ReferenceStatus.Created,
            DateTime? createdAt = null,
            DateTime? updatedAt = null,
            DateTime? closedAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Reference>($"{nameof(id)} не может быть пустым");
            }
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<Reference>($"{nameof(employeeId)} не может быть пустым");
            }
            if (copiesCount < 1)
            {
                return Result.Failure<Reference>($"{nameof(copiesCount)} не может равным нулю или отрицательным");
            }
            if (string.IsNullOrWhiteSpace(reason) || reason.Length > REASON_MAX_LENGTH)
            {
                return Result.Failure<Reference>($"{nameof(reason)} не может быть пустым или длиннее {REASON_MAX_LENGTH}");
            }
            if (status == ReferenceStatus.Closed && closedAt is null)
            {
                return Result.Failure<Reference>($"{nameof(closedAt)} должен быть задан для закрытой заявки");
            }
            if (status != ReferenceStatus.Closed && closedAt is not null)
            {
                return Result.Failure<Reference>($"{nameof(closedAt)} можно задавать только для закрытой заявки");
            }

            var certificateRequest = new Reference(
                id,
                employeeId,
                type,
                copiesCount,
                reason,
                status,
                createdAt ?? DateTime.UtcNow,
                updatedAt,
                closedAt);

            return Result.Success(certificateRequest);
        }

        /// <summary>
        /// Обновляет статус заявки с учетом допустимых переходов.
        /// </summary>
        /// <param name="status">Новый статус заявки.</param>
        /// <returns>Результат операции обновления статуса.</returns>
        public Result UpdateStatus(ReferenceStatus status)
        {
            if (Status == ReferenceStatus.Closed)
            {
                return Result.Failure("Закрытую заявку нельзя изменить");
            }

            if (Status == ReferenceStatus.InProgress && status == ReferenceStatus.Created)
            {
                return Result.Failure("Заявку в работе нельзя вернуть в статус создана");
            }

            if (Status == status)
            {
                return Result.Failure("Заявка уже находится в этом статусе");
            }

            var now = DateTime.UtcNow;

            Status = status;
            UpdatedAt = now;

            if (status == ReferenceStatus.Closed)
            {
                ClosedAt = now;
            }
            return Result.Success();
        }
    }
}
