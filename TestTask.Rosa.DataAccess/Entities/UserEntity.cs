using TestTask.Rosa.Core.Enums;

namespace TestTask.Rosa.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public ICollection<ReferenceEntity> References { get; set; } = new List<ReferenceEntity>();
    }
}
