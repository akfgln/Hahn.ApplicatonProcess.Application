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
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("Asset");

                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Id)
                     .HasName("PK__Users");

                entity.ToTable("Users");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.EMail)
                   .HasColumnName("EMail")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.FirstName)
                   .HasColumnName("FirstName")
                   .HasMaxLength(1500)
                   .IsUnicode(false);

                entity.Property(e => e.LastName)
                   .HasColumnName("LastName")
                   .HasMaxLength(1500)
                   .IsUnicode(false);

                entity.Property(e => e.Password)
                   .HasColumnName("Password")
                   .HasMaxLength(1500)
                   .IsUnicode(false);

            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK___user__FA7E257D289427F7");

                entity.ToTable("UserRoles");

                entity.HasIndex(e => e.RoleId).HasName("FKlw289ua9wro1sxs07g3da0p1i");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleId");

                entity.Property(e => e.IsActive)
                    .HasColumnName("IsActive")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.RoleId).HasColumnName("RoleId");

                entity.HasOne(e => e.Role).WithMany(u => u.UserRoles)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.HasOne(e => e.User).WithMany(u => u.Roles)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            });
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK___role__5AC4D22297C45B90");

                entity.ToTable("Roles");

                entity.Property(e => e.RoleId).HasColumnName("RoleId");

                entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");

                entity.Property(e => e.CreateDate).HasColumnName("CreateDate");

                entity.Property(e => e.DefaultRoleName)
                    .HasColumnName("DefaultRoleName")
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("IsDeleted")
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.HasMany(e => e.UserRoles).WithOne(x => x.Role);
            });

            //OnModelCreatingPartial(modelBuilder);
        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //           .UseLazyLoadingProxies();
        //}
    }
}
