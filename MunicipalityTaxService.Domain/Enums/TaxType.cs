namespace MunicipalityTaxService.Domain.Enums;

/// <remarks>
/// Assumption: the precedence order Daily; Weekly; Monthly; Yearly is inferred
/// from the task example (a daily rate overrides a yearly one; a monthly rate
/// overrides a yearly one and etc.). Weekly is placed between Daily and Monthly by granularity.
/// </remarks>
public enum TaxType
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}
