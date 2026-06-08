using MunicipalityTaxService.Domain.Enums;

namespace MunicipalityTaxService.Api.Dtos;

public class TaxRateBaseDto
{
    public TaxType Type { get; set; }

    public decimal Rate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
