using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Rosa.Core.Models;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess.Configurations
{
    /// <summary>
    /// Конфигурация таблицы заявок на справки.
    /// </summary>
    public class ReferenceConfigurations : IEntityTypeConfiguration<ReferenceEntity>
    {
        /// <summary>
        /// Настраивает свойства, ограничения и связи сущности заявки.
        /// </summary>
        /// <param name="builder">Построитель конфигурации сущности.</param>
        public void Configure(EntityTypeBuilder<ReferenceEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.CopiesCount)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(Reference.REASON_MAX_LENGTH)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            builder.Property(x => x.ClosedAt)
                .IsRequired(false);

            builder
                .ToTable(x => x.HasCheckConstraint("CK_Reference_CopiesCount", "\"CopiesCount\" > 0"));

            builder.HasOne(x => x.User)
                .WithMany(x => x.References)
                .HasForeignKey(x => x.UserId);
        }
    }
}
