using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ASPIdentity.Data
{
    public partial class ASPIdentityContext : IdentityDbContext<ApplicationUser,ApplicationRole, Guid>
    {
        public ASPIdentityContext()
        {
        }

        public ASPIdentityContext(DbContextOptions<ASPIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
