using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicatonProcess.February2021.Data
{
    public class HahnDbContext : DbContext
    {
        public HahnDbContext(DbContextOptions<HahnDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Asset> Asset { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset");

                entity.Property(e => e.Id).HasColumnName("Id");
            });
        }
    }
}
