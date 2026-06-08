using Microsoft.EntityFrameworkCore;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Infrastructure.Repositories;

public sealed class MunicipalityRepository : IMunicipalityRepository
{
    private readonly ServiceDbContext _dbContext;

    public MunicipalityRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public async Task<Municipality> AddMunicipalityAsync(Municipality municipality, CancellationToken cancellationToken)
    {
        await _dbContext.Set<Municipality>().AddAsync(municipality, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return municipality;
    }

    public async Task<Municipality?> GetMunicipalityByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Municipality>()
            .AsNoTracking()
            .FirstOrDefaultAsync(municipality => municipality.Id == id, cancellationToken);
    }

    public async Task<Municipality?> GetMunicipalityByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Municipality>()
            .AsNoTracking()
            .FirstOrDefaultAsync(municipality => municipality.Name == name, cancellationToken);
    }
}
