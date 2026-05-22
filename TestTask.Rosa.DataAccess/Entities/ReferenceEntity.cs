using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.DataAccess.Entities
{
    public class ReferenceEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public ReferenceType Type { get; set; }
        public int CopiesCount { get; set; } = 0;
        public string Reason { get; set; } = string.Empty;
        public ReferenceStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

    }
}
