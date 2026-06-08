using MunicipalityTaxService.Application.Errors;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Services;

public sealed class MunicipalityService : IMunicipalityService
{
    private readonly IMunicipalityRepository _municipalityRepository;

    public MunicipalityService(IMunicipalityRepository municipalityRepository)
    {
        _municipalityRepository = municipalityRepository;
    }

    public async Task<OperationResult<Municipality>> AddMunicipalityAsync(Municipality municipality, CancellationToken cancellationToken)
    {
        var existingMunicipality = await _municipalityRepository.GetMunicipalityByNameAsync(municipality.Name, cancellationToken);
        if (existingMunicipality is not null)
        {
            return OperationResult.Failure<Municipality>(MunicipalityErrors.AlreadyExists(municipality.Name));
        }

        var createdMunicipality = await _municipalityRepository.AddMunicipalityAsync(municipality, cancellationToken);
        return OperationResult.Success(createdMunicipality);
    }

    public async Task<OperationResult<Municipality>> GetMunicipalityByIdAsync(long id, CancellationToken cancellationToken)
    {
        var municipality = await _municipalityRepository.GetMunicipalityByIdAsync(id, cancellationToken);

        return municipality is not null
            ? OperationResult.Success(municipality)
            : OperationResult.Failure<Municipality>(MunicipalityErrors.NotFoundById(id));
    }
}
