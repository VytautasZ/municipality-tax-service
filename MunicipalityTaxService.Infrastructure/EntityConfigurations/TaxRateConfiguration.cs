using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Infrastructure.Configurations;

namespace MunicipalityTaxService.Infrastructure.EntityConfigurations;

internal sealed class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.ToTable(Db.MunicipalityTax.Tables.TaxRates, Db.MunicipalityTax.SCHEMA);

        builder.HasKey(taxRate => taxRate.Id);
        builder.Property(taxRate => taxRate.Type).IsRequired();
        builder.Property(taxRate => taxRate.Rate).IsRequired().HasColumnType("decimal(7,4)");
        builder.Property(taxRate => taxRate.StartDate).IsRequired();
        builder.Property(taxRate => taxRate.EndDate).IsRequired();
        builder.Property(taxRate => taxRate.CreatedAt).IsRequired();
        builder.Property(taxRate => taxRate.UpdatedAt).IsRequired();
    }
}
