using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Api.Mappers;
using MunicipalityTaxService.Application.Interfaces;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Endpoints for querying the tax rates that apply to a municipality.
/// </summary>
[ApiController]
[Route("api/municipalities/{name}/tax-rates")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Municipality Tax Rates")]
public class MunicipalityTaxRatesController : ControllerBase
{
    private readonly ITaxRateService _taxRateService;

    public MunicipalityTaxRatesController(ITaxRateService taxRateService)
    {
        _taxRateService = taxRateService;
    }

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
        var taxRate = await _taxRateService.GetMunicipalityTaxRateByDateAsync(name, date, cancellationToken);

        return taxRate is not null
            ? Ok(TaxRateMapper.ToDto(name, taxRate))
            : NotFound();
    }

    /// <summary>
    /// Adds a tax rate for a municipality. The municipality is created if it does not yet exist.
    /// </summary>
    /// <param name="name">The municipality name (case-insensitive).</param>
    /// <param name="taxRateDto">The tax rate to add.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">The tax rate was created.</response>
    [HttpPost]
    [ProducesResponseType<TaxRateDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<TaxRateDto>> AddTaxRate(
        string name,
        [FromBody] TaxRateDto taxRateDto,
        CancellationToken cancellationToken)
    {
        var createdTaxRate = await _taxRateService.AddTaxRateAsync(name, taxRateDto.ToTaxRate(), cancellationToken);
        return Ok(createdTaxRate);
    }

    /// <summary>
    /// Updates an existing tax rate.
    /// </summary>
    /// <param name="name">The municipality name (case-insensitive).</param>
    /// <param name="id">The id of the tax rate to update.</param>
    /// <param name="taxRateDto">The new tax rate values.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">The tax rate was updated.</response>
    /// <response code="404">No tax rate with the given id exists.</response>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaxRate(
        string name,
        long id,
        [FromBody] TaxRateDto taxRateDto,
        CancellationToken cancellationToken)
    {
        var updated = await _taxRateService.UpdateTaxRateAsync(id, taxRateDto.ToTaxRate(), cancellationToken);

        return updated ? NoContent() : NotFound();
    }
}
