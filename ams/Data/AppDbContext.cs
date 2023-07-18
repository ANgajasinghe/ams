using ams.Entities;
using Microsoft.EntityFrameworkCore;

namespace ams;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) :base(options) { }
    
    public DbSet<Allowance> Allowances => Set<Allowance>();
    public DbSet<UploadHistory> UploadHistories => Set<UploadHistory>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();
    }
}