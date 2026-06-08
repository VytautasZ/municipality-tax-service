using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Services;

public sealed class TaxRateService : ITaxRateService
{
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly ITaxRateRepository _taxRateRepository;

    public TaxRateService(IMunicipalityRepository municipalityRepository, ITaxRateRepository taxRateRepository)
    {
        _municipalityRepository = municipalityRepository;
        _taxRateRepository = taxRateRepository;
    }

    public async Task<TaxRate?> GetMunicipalityTaxRateByDateAsync(string municipalityName, DateTime date, CancellationToken cancellationToken)
    {
        var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName, cancellationToken);
        if (municipality is null)
        {
            return null;
        }

        IReadOnlyList<TaxRate> validTaxRates = await _taxRateRepository.GetMunicipalityTaxRatesByDateAsync(municipality.Id, date, cancellationToken);

        return validTaxRates
            .OrderBy(taxRate => taxRate.Type)
            .ThenByDescending(taxRate => taxRate.CreatedAt) 
            .FirstOrDefault();
    }
}
