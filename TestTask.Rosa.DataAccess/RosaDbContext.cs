using Microsoft.EntityFrameworkCore;
using TestTask.Rosa.DataAccess.Entities;

namespace TestTask.Rosa.DataAccess
{
    public class RosaDbContext : DbContext
    {
        public RosaDbContext(DbContextOptions<RosaDbContext> options) 
            : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ReferenceEntity> References { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RosaDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
