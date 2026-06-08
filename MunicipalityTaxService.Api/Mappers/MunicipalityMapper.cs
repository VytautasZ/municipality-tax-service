using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Api.Mappers;

public static class MunicipalityMapper
{
    public static Municipality ToMunicipality(this MunicipalityDto dto)
    {
        return new Municipality
        {
            Id = dto.Id,
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
