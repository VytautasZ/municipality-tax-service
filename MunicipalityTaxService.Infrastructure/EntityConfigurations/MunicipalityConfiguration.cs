using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Infrastructure.Configurations;

namespace MunicipalityTaxService.Infrastructure.EntityConfigurations;

internal sealed class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder.ToTable(Db.MunicipalityTax.Tables.Municipalities, Db.MunicipalityTax.SCHEMA);

        builder.HasKey(municipality => municipality.Id);
        builder.Property(municipality => municipality.Name).IsRequired().HasMaxLength(200);

        builder.HasIndex(municipality => municipality.Name).IsUnique();

        builder.HasMany(municipality => municipality.TaxRates)
               .WithOne()
               .HasForeignKey(taxRate => taxRate.MunicipalityId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
