namespace MunicipalityTaxService.Domain.Enums;

/// <remarks>
/// Assumption: the precedence order Daily; Weekly; Monthly; Yearly is inferred
/// from the task example (a daily rate overrides a yearly one; a monthly rate
/// overrides a yearly one and etc.). Weekly is placed between Daily and Monthly by granularity.
/// </remarks>
public enum TaxType
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Yearly = 3,
}
