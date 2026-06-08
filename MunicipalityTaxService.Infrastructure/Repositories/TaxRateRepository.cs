using Microsoft.EntityFrameworkCore;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Infrastructure.Repositories;

public sealed class TaxRateRepository : ITaxRateRepository
{
    private readonly ServiceDbContext _dbContext;

    public TaxRateRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TaxRate>> GetMunicipalityTaxRatesByDateAsync(long municipalityId, DateTime date, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<TaxRate>()
            .AsNoTracking()
            .Where(taxRate => taxRate.MunicipalityId == municipalityId
                              && taxRate.StartDate <= date
                              && date <= taxRate.EndDate)
            .ToListAsync(cancellationToken);
    }
}
