using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Api.Mappers;

public static class MunicipalityMapper
{
    public static Municipality ToMunicipality(this MunicipalityBaseDto dto)
    {
        return new Municipality
        {
            Name = dto.Name
        };
    }

    public static MunicipalityDto ToMunicipalityDto(this Municipality municipality)
    {
        return new MunicipalityDto
        {
            Id = municipality.Id,
            Name = municipality.Name
        };
    }
}
