using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Api.Mappers;
using MunicipalityTaxService.Application.Interfaces;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Endpoints for managing municipalities.
/// </summary>
[ApiController]
[Route("api/municipalities")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Municipalities")]
public class MunicipalitiesController : ApiController
{
    private readonly IMunicipalityService _municipalityService;

    public MunicipalitiesController(IMunicipalityService municipalityService)
    {
        _municipalityService = municipalityService;
    }

    /// <summary>
    /// Adds a new municipality.
    /// </summary>
    /// <param name="municipalityDto">The municipality to add.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">The municipality was created.</response>
    [HttpPost]
    [ProducesResponseType<MunicipalityDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<MunicipalityDto>> AddMunicipality(
        [FromBody] MunicipalityDto municipalityDto,
        CancellationToken cancellationToken)
    {
        var result = await _municipalityService.AddMunicipalityAsync(municipalityDto.ToMunicipality(), cancellationToken);
        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var createdMunicipality = result.Value;
        return CreatedAtAction(nameof(GetMunicipality), new { id = createdMunicipality.Id }, createdMunicipality.ToMunicipalityDto());
    }

    /// <summary>
    /// Gets a municipality by id.
    /// </summary>
    /// <param name="id">The municipality id.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">The municipality.</response>
    /// <response code="404">No municipality with the given id exists.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType<MunicipalityDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MunicipalityDto>> GetMunicipality(
        long id,
        CancellationToken cancellationToken)
    {
        var result = await _municipalityService.GetMunicipalityByIdAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value.ToMunicipalityDto())
            : Problem(result.Error);
    }
}
