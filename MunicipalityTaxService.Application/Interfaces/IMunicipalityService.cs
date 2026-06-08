using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Interfaces;

public interface IMunicipalityService
{
    Task<OperationResult<Municipality>> AddMunicipalityAsync(Municipality municipality, CancellationToken cancellationToken);

    Task<OperationResult<Municipality>> GetMunicipalityByIdAsync(long id, CancellationToken cancellationToken);
}
