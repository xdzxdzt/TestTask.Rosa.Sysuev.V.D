using CSharpFunctionalExtensions;
using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.Core.Models
{
    public class Reference
    {
        public const int REASON_MAXLENGTH = 5000;

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

        public Guid Id {  get; private set; }
        public Guid EmployeeId { get; private set; }
        public ReferenceType Type { get; private set; }
        public int CopiesCount { get; private set; }
        public string Reason { get; private set; }
        public ReferenceStatus Status { get; private set; }
        public DateTime CreatedAt {  get; private set; }
        public DateTime? UpdatedAt {  get; private set; }
        public DateTime? ClosedAt { get; private set; }

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
            if(id == Guid.Empty)
            {
                return Result.Failure<Reference>($"{nameof(id)} не может быть пустым");
            }
            if (employeeId == Guid.Empty)
            {
                return Result.Failure<Reference>($"{nameof(employeeId)} не может быть пустым");
            }
            if(copiesCount < 1)
            {
                return Result.Failure<Reference>($"{nameof(copiesCount)} не может равным нулю или отрицательным");
            }
            if (string.IsNullOrWhiteSpace(reason) || reason.Length > REASON_MAXLENGTH)
            {
                return Result.Failure<Reference>($"{nameof(reason)} не может быть пустым или длиннее {REASON_MAXLENGTH}");
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

        public Result UpdateStatus(ReferenceStatus status)
        {
            if (Status == ReferenceStatus.Closed)
            {
                return Result.Failure("Закрытую заявку нельзя изменить");
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