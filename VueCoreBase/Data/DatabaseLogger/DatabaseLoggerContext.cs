using Microsoft.EntityFrameworkCore;


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

        public virtual DbSet<CalDavLog> CalDavLog { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<RequestLog> RequestLog { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<CalDavLog>(entity =>
            {
                entity.ToTable("CalDavLog", "DatabaseLogger");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Request).IsRequired();

                entity.Property(e => e.Response).IsRequired();

                entity.Property(e => e.ResponseContentType).HasMaxLength(256);
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("ErrorLog", "DatabaseLogger");

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

            modelBuilder.Entity<RequestLog>(entity =>
            {
                entity.ToTable("RequestLog", "DatabaseLogger");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.ToTable("UserLog", "DatabaseLogger");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TokenType).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });
        }
    }
}
