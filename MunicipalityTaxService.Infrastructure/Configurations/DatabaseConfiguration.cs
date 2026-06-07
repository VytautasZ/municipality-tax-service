using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MunicipalityTaxService.Infrastructure.Configurations;

internal static class DatabaseConfiguration
{
    internal static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString(Db.MunicipalityTax.CONNECTION_STRING_NAME);
        services.AddDbContext<ServiceDbContext>(options => options.UseSqlServer(connectionString,
            sql => sql.MigrationsHistoryTable("_MigrationHistory", Db.MunicipalityTax.SCHEMA)));
        return services;
    }
}
