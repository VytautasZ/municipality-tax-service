using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Api.Mappers;
using MunicipalityTaxService.Application.Interfaces;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Endpoints for querying and managing the tax rates of a municipality.
/// </summary>
[ApiController]
[Route("api/municipalities/{name}/tax-rates")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Municipality Tax Rates")]
public class MunicipalityTaxRatesController : ApiController
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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaxRateDto>> GetApplicableTaxRate(
        string name,
        [FromQuery] DateTime date,
        CancellationToken cancellationToken)
    {
        var result = await _taxRateService.GetMunicipalityTaxRateByDateAsync(name, date, cancellationToken);

        return result.IsSuccess
            ? Ok(TaxRateMapper.ToDto(name, result.Value))
            : Problem(result.Error);
    }

    /// <summary>
    /// Adds a tax rate for a municipality.
    /// </summary>
    /// <param name="name">The municipality name (case-insensitive).</param>
    /// <param name="taxRateDto">The tax rate to add.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">The tax rate was created.</response>
    /// <response code="404">No municipality with the given name exists.</response>
    [HttpPost]
    [ProducesResponseType<TaxRateDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaxRateDto>> AddTaxRate(
        string name,
        [FromBody] TaxRateBaseDto taxRateDto,
        CancellationToken cancellationToken)
    {
        var result = await _taxRateService.AddTaxRateAsync(name, taxRateDto.ToTaxRate(), cancellationToken);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var createdTaxRate = result.Value;
        return CreatedAtAction(nameof(GetApplicableTaxRate), new { name, date = createdTaxRate.StartDate }, TaxRateMapper.ToDto(name, createdTaxRate));
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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaxRate(
        string name,
        long id,
        [FromBody] TaxRateDto taxRateDto,
        CancellationToken cancellationToken)
    {
        var result = await _taxRateService.UpdateTaxRateAsync(id, taxRateDto.ToTaxRate(), cancellationToken);
        return result.IsSuccess ? NoContent() : Problem(result.Error);
    }
}
