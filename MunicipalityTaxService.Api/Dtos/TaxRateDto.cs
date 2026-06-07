using MunicipalityTaxService.Domain.Enums;

namespace MunicipalityTaxService.Api.Dtos;

public class TaxRateDto
{
    public long Id { get; set; }

    public required string MunicipalityName { get; set; }

    public TaxType Type { get; set; }

    public decimal Rate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
