using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Rosa.Core.Models;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasMaxLength(User.MAX_LENGTH_FIRSTNAME)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(User.MAX_LENGTH_LASTNAME)
                .IsRequired();

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
