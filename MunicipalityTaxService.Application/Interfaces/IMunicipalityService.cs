using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Application.Interfaces;

public interface IMunicipalityService
{
    Task<Municipality> AddMunicipalityAsync(Municipality municipality, CancellationToken cancellationToken);

    Task<Municipality?> GetMunicipalityByIdAsync(long id, CancellationToken cancellationToken);
}
