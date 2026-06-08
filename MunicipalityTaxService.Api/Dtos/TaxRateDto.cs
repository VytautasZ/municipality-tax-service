using MunicipalityTaxService.Domain.Enums;

namespace MunicipalityTaxService.Api.Dtos;

public class TaxRateDto : TaxRateBaseDto
{
    public long Id { get; set; }
    public required string MunicipalityName { get; set; }
}
