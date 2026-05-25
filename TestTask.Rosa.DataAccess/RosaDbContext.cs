using Microsoft.EntityFrameworkCore;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess
{
    /// <summary>
    /// Контекст базы данных приложения Rosa.
    /// </summary>
    public class RosaDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных.
        /// </summary>
        /// <param name="options">Параметры настройки контекста.</param>
        public RosaDbContext(DbContextOptions<RosaDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// Набор пользователей.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Набор заявок на справки.
        /// </summary>
        public DbSet<ReferenceEntity> References { get; set; }

        /// <summary>
        /// Применяет конфигурации моделей при создании схемы.
        /// </summary>
        /// <param name="modelBuilder">Построитель модели Entity Framework.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RosaDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
