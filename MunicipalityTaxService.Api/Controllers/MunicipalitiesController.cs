using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Api.Mappers;
using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Endpoints for managing municipalities.
/// </summary>
[ApiController]
[Route("api/municipalities")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Municipalities")]
public class MunicipalitiesController : ControllerBase
{
    private readonly IMunicipalityService _municipalityService;

    public MunicipalitiesController(IMunicipalityService municipalityService)
    {
        _municipalityService = municipalityService;
    }

    /// <summary>
    /// Adds a new municipality.
    /// </summary>
    /// <param name="request">The municipality to add.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="201">The municipality was created.</response>
    [HttpPost]
    [ProducesResponseType<MunicipalityDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<MunicipalityDto>> AddMunicipality(
        [FromBody] MunicipalityDto municipalityDto,
        CancellationToken cancellationToken)
    {
        var createdMunicipality = await _municipalityService.AddMunicipalityAsync(municipalityDto.ToMunicipality(), cancellationToken);

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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MunicipalityDto>> GetMunicipality(
        long id,
        CancellationToken cancellationToken)
    {
        var municipality = await _municipalityService.GetMunicipalityByIdAsync(id, cancellationToken);

        return municipality is not null
            ? Ok(municipality.ToMunicipalityDto())
            : NotFound();
    }
}
