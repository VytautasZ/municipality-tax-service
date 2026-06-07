using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Api.Mappers;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Endpoints for querying the tax rates that apply to a municipality.
/// </summary>
[ApiController]
[Route("api/municipalities/{name}/tax-rates")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Municipality Tax Rates")]
public class MunicipalityTaxesController(ITaxRateService taxRateService) : ControllerBase
{
    /// <summary>
    /// Gets the tax rate that applies to a municipality on a specific date by municipality name.
    /// </summary>
    /// <remarks>
    /// When several tax rates are valid on the same date, the one with the finest
    /// granularity wins (Daily &gt; Weekly &gt; Monthly &gt; Yearly).
    /// </remarks>
    /// <param name="name">The municipality name (case-insensitive).</param>
    /// <param name="date">The date to resolve the applicable tax rate for.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">The applicable tax rate for the municipality on the given date.</response>
    /// <response code="404">No tax rate is set for the municipality on the given date.</response>
    [HttpGet]
    [ProducesResponseType<TaxRateDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaxRateDto>> GetApplicableTaxRate(
        string name,
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        var taxRate = await taxRateService.GetApplicableTaxRateAsync(name, date, cancellationToken);

        return taxRate is not null
            ? Ok(TaxRateMapper.ToDto(name, taxRate))
            : NotFound();
    }
}

// TODO: Implement ITaxRateService in the application layer, and register it in the dependency injection container and remove Domain project reference.

public interface ITaxRateService
{
    Task<TaxRate?> GetApplicableTaxRateAsync(string name, DateTime date, CancellationToken cancellationToken);
}
