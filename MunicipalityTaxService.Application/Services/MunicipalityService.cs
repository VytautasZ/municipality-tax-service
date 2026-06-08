using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Services;

public sealed class MunicipalityService : IMunicipalityService
{
    private readonly IMunicipalityRepository _municipalityRepository;

    public MunicipalityService(IMunicipalityRepository municipalityRepository)
    {
        _municipalityRepository = municipalityRepository;
    }

    public Task<Municipality> AddMunicipalityAsync(Municipality municipality, CancellationToken cancellationToken)
    {
        return _municipalityRepository.AddMunicipalityAsync(municipality, cancellationToken);
    }

    public Task<Municipality?> GetMunicipalityByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _municipalityRepository.GetMunicipalityByIdAsync(id, cancellationToken);
    }
}
