using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POSM.Domain.User;
using System;
using System.Threading.Tasks;

namespace POSM.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, CustomRole, Guid>, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>(
                entity =>
                {
                    entity.ToTable(name: "User");
                    entity.Property(e => e.Id).HasColumnName("Id");
                    entity.HasMany(p => p.Supervisees)
                        .WithOne(b => b.Supervior)
                        .HasForeignKey(p => p.SupervisorId)
                        .IsRequired(false)
                        .OnDelete(DeleteBehavior.ClientSetNull);
                });

        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
