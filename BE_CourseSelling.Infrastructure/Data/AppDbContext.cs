using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BE_CourseSelling.Infrastructure.Entities;
using BE_CourseSelling.Core.Entities;
using BE_CourseSelling.Core.Common;

namespace BE_CourseSelling.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
   : base(options)
    {
    }

    public DbSet<CourseDTO> Courses => Set<CourseDTO>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
