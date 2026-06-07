using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface ITaxRateService
{
    Task<TaxRate?> GetMunicipalityTaxRateAsync(string municipalityName, DateTime date, CancellationToken cancellationToken);
}
