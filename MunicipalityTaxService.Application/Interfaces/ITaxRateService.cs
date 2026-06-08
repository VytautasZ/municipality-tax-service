using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface ITaxRateService
{
    Task<TaxRate> AddTaxRateAsync(string municipalityName, TaxRate taxRate, CancellationToken cancellationToken);

    Task<bool> UpdateTaxRateAsync(long id, TaxRate taxRate, CancellationToken cancellationToken);

    Task<TaxRate?> GetMunicipalityTaxRateByDateAsync(string municipalityName, DateTime date, CancellationToken cancellationToken);
}
