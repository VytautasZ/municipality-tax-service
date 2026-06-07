using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface IMunicipalityRepository
{
    Task<Municipality?> GetByNameAsync(string municipalityName, CancellationToken cancellationToken);
}
