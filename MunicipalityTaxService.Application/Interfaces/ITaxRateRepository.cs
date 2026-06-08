using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface ITaxRateRepository
{
    Task<TaxRate> AddTaxRateAsync(TaxRate taxRate, CancellationToken cancellationToken);

    Task<bool> UpdateTaxRateAsync(long id, TaxRate taxRate, CancellationToken cancellationToken);

    Task<IReadOnlyList<TaxRate>> GetMunicipalityTaxRatesByDateAsync(long municipalityId, DateTime date, CancellationToken cancellationToken);
}
