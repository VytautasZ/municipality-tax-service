using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Errors;

public static class TaxRateErrors
{
    public static Error NotFoundById(long id) =>
        Error.NotFound("TaxRate.NotFound", $"No tax rate with id '{id}' was found.");

    public static Error NotApplicable(string municipalityName, DateTime date) =>
        Error.NotFound("TaxRate.NotApplicable", $"No tax rate is set for '{municipalityName}' on {date:yyyy-MM-dd}.");

    public static readonly Error InvalidDateRange =
        Error.Validation("TaxRate.InvalidDateRange", "The start date must be on or before the end date.");

    public static readonly Error NegativeRate =
        Error.Validation("TaxRate.NegativeRate", "The tax rate must be zero or greater.");
}
