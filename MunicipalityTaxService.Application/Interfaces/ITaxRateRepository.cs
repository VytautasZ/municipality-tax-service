using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface ITaxRateRepository
{
    Task<IReadOnlyList<TaxRate>> GetTaxRatesByMunicipalityIdAsync(long municipalityId, DateTime date, CancellationToken cancellationToken);
}
