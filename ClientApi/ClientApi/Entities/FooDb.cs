using Microsoft.EntityFrameworkCore;

namespace ClientApi.Entities
{
    public partial class FooDb : DbContext
    {
        public FooDb()
        {
        }

        public FooDb(DbContextOptions<FooDb> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Environment> Environments { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<Version> Versions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Environment)
                    .WithMany()
                    .HasForeignKey(d => d.EnvironmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Owner)
                    .WithMany()
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Stage)
                    .WithMany()
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Type)
                    .WithMany()
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Version)
                    .WithMany()
                    .HasForeignKey(d => d.VersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Environment>(entity =>
            {
                entity.ToTable("Environment");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.ToTable("Stage");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.ToTable("Version");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
