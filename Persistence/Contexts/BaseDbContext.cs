using System.Reflection;
using Domain.BaseProjeEntities.FileEntities;
using Domain.BaseProjeEntities.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = Domain.BaseProjeEntities.FileEntities.File;

namespace Persistence.Contexts;

public class BaseDbContext : IdentityDbContext<AppUser,AppRole,Guid>
{
   

    public DbSet<File> Files { get; set; }
    public DbSet<ImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Endpoint> Endpoints { get; set; }


    public BaseDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Assembly'deki tüm IEntityTypeConfiguration'ları uygula
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }
}

