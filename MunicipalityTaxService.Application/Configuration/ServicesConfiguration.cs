using Microsoft.Extensions.DependencyInjection;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Application.Services;

namespace MunicipalityTaxService.Application.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaxRateService, TaxRateService>();
        return services;
    }
}
