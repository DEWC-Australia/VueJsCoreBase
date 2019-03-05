using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.DatabaseLogger
{
    public partial class DatabaseLoggerContext : DbContext
    {
        public DatabaseLoggerContext()
        {
        }

        public DatabaseLoggerContext(DbContextOptions<DatabaseLoggerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DatabaseLog> DatabaseLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<DatabaseLog>(entity =>
            {
                entity.ToTable("DatabaseLog", "DatabaseLogger");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Callsite).HasMaxLength(300);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Logger).HasMaxLength(300);

                entity.Property(e => e.MachineName).HasMaxLength(200);

                entity.Property(e => e.Message).IsRequired();
            });
        }
    }
}
