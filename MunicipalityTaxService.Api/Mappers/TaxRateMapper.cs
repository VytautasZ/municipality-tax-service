using MunicipalityTaxService.Api.Dtos;
using MunicipalityTaxService.Domain.Entities;

namespace MunicipalityTaxService.Api.Mappers;

public static class TaxRateMapper
{
    public static TaxRate ToTaxRate(this TaxRateDto dto)
    {
        return new TaxRate
        {
            Type = dto.Type,
            Rate = dto.Rate,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public static TaxRateDto ToDto(string municipalityName, TaxRate taxRate)
    {
        return new()
        {
            Id = taxRate.Id,
            MunicipalityName = municipalityName,
            Type = taxRate.Type,
            Rate = taxRate.Rate,
            StartDate = taxRate.StartDate,
            EndDate = taxRate.EndDate
        };
    }
}
