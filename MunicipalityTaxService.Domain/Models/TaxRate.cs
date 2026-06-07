using MunicipalityTaxService.Domain.Enums;

namespace MunicipalityTaxService.Domain.Entities;

public class TaxRate
{
    public long Id { get; set; }

    public long MunicipalityId { get; set; }

    public TaxType Type { get; set; }

    public decimal Rate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
