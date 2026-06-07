using Microsoft.EntityFrameworkCore;
using MunicipalityTaxService.Infrastructure.EntityConfigurations;

namespace MunicipalityTaxService.Infrastructure;

public sealed class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MunicipalityConfiguration());
        modelBuilder.ApplyConfiguration(new TaxRateConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
