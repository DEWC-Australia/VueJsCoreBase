using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.VueCoreBase
{
    public partial class VueCoreBaseContext : DbContext
    {
        public VueCoreBaseContext()
        {
        }

        public VueCoreBaseContext(DbContextOptions<VueCoreBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Logging> Logging { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logging>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("Logging", "VueCoreBase");

                entity.Property(e => e.LogId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.RequestMethod)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestPath).IsRequired();
            });
        }
    }
}
