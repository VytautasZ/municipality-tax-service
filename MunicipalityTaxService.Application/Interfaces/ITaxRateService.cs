using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Interfaces;

public interface ITaxRateService
{
    Task<OperationResult<TaxRate>> AddTaxRateAsync(string municipalityName, TaxRate taxRate, CancellationToken cancellationToken);

    Task<OperationResult> UpdateTaxRateAsync(long id, TaxRate taxRate, CancellationToken cancellationToken);

    Task<OperationResult<TaxRate>> GetMunicipalityTaxRateByDateAsync(string municipalityName, DateTime date, CancellationToken cancellationToken);
}
