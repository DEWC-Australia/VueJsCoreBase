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
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database='VueCoreBase'; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<DatabaseLog>(entity =>
            {
                entity.ToTable("DatabaseLog", "DatabaseLogger");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Callsite).HasMaxLength(300);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Logger).HasMaxLength(300);

                entity.Property(e => e.MachineName).HasMaxLength(200);

                entity.Property(e => e.Message).IsRequired();
            });
        }
    }
}
