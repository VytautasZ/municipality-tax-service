using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Errors;

public static class TaxRateErrors
{
    public static Error NotFoundById(long id) =>
        Error.NotFound("TaxRate.NotFound", $"No tax rate with id '{id}' was found.");

    public static Error NotApplicable(string municipalityName, DateTime date) =>
        Error.NotFound("TaxRate.NotApplicable", $"No tax rate is set for '{municipalityName}' on {date:yyyy-MM-dd}.");
}
