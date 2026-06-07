namespace MunicipalityTaxService.Domain.Entities;

public class Municipality
{
    public long Id { get; set; }

    public string Name { get; set; }

    public List<TaxRate> TaxRates { get; set; }
}
