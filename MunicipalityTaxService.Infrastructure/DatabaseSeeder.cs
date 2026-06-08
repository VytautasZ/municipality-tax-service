using Microsoft.EntityFrameworkCore;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Domain.Enums;

namespace MunicipalityTaxService.Infrastructure;

/// <summary>
/// Seeds the example data from the task (Copenhagen's tax schedule) the first time the
/// application runs against an empty database. Subsequent runs are a no-op.
/// </summary>
internal static class DatabaseSeeder
{
    public static async Task SeedAsync(ServiceDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Set<Municipality>().AnyAsync(cancellationToken))
        {
            return;
        }

        DateTime timestamp = DateTime.UtcNow;

        var copenhagen = new Municipality
        {
            Name = "Copenhagen",
            TaxRates =
            [
                new TaxRate
                {
                    Type = TaxType.Yearly,
                    Rate = 0.2m,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp
                },
                new TaxRate
                {
                    Type = TaxType.Monthly,
                    Rate = 0.4m,
                    StartDate = new DateTime(2024, 5, 1),
                    EndDate = new DateTime(2024, 5, 31),
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp
                },
                new TaxRate
                {
                    Type = TaxType.Daily,
                    Rate = 0.1m,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 1, 1),
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp
                },
                new TaxRate
                {
                    Type = TaxType.Daily,
                    Rate = 0.1m,
                    StartDate = new DateTime(2024, 12, 25),
                    EndDate = new DateTime(2024, 12, 25),
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp
                }
            ]
        };

        await dbContext.Set<Municipality>().AddAsync(copenhagen, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
