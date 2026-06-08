using MunicipalityTaxService.Application.Errors;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Shared;

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

    public async Task<OperationResult<TaxRate>> AddTaxRateAsync(string municipalityName, TaxRate taxRate, CancellationToken cancellationToken)
    {
        var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName, cancellationToken);
        if (municipality is null)
        {
            return OperationResult.Failure<TaxRate>(MunicipalityErrors.NotFoundByName(municipalityName));
        }

        taxRate.MunicipalityId = municipality.Id;
        var createdTaxRate = await _taxRateRepository.AddTaxRateAsync(taxRate, cancellationToken);
        return OperationResult.Success(createdTaxRate);
    }

    public async Task<OperationResult> UpdateTaxRateAsync(long id, TaxRate taxRate, CancellationToken cancellationToken)
    {
        var updated = await _taxRateRepository.UpdateTaxRateAsync(id, taxRate, cancellationToken);

        return updated
            ? OperationResult.Success()
            : OperationResult.Failure(TaxRateErrors.NotFoundById(id));
    }

    public async Task<OperationResult<TaxRate>> GetMunicipalityTaxRateByDateAsync(string municipalityName, DateTime date, CancellationToken cancellationToken)
    {
        var municipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipalityName, cancellationToken);
        if (municipality is null)
        {
            return OperationResult.Failure<TaxRate>(MunicipalityErrors.NotFoundByName(municipalityName));
        }

        var validTaxRates = await _taxRateRepository.GetMunicipalityTaxRatesByDateAsync(municipality.Id, date, cancellationToken);

        var applicableTaxRate = validTaxRates
            .OrderBy(taxRate => taxRate.Type)
            .ThenByDescending(taxRate => taxRate.CreatedAt)
            .FirstOrDefault();

        return applicableTaxRate is not null
            ? OperationResult.Success(applicableTaxRate)
            : OperationResult.Failure<TaxRate>(TaxRateErrors.NotApplicable(municipalityName, date));
    }
}
