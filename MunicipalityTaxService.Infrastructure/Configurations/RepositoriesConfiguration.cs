using Microsoft.Extensions.DependencyInjection;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Infrastructure.Repositories;

namespace MunicipalityTaxService.Infrastructure.Configurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
    {
        services.AddTransient<IMunicipalityRepository, MunicipalityRepository>();
        services.AddTransient<ITaxRateRepository, TaxRateRepository>();
        return services;
    }
}
