namespace MunicipalityTaxService.Infrastructure.Configurations;

public static class Db
{
    public static class MunicipalityTax
    {
        public const string SCHEMA = "municipalityTax";
        public const string CONNECTION_STRING_NAME = "MunicipalityTaxDbString";

        public static class Tables
        {
            public const string Municipalities = "Municipalities";
            public const string TaxRates = "TaxRates";
        }
    }
}
