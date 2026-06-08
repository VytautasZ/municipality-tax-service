using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Application.Services;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Domain.Enums;
using MunicipalityTaxService.Shared;
using NSubstitute;

namespace MunicipalityTaxService.UnitTests;

public class TaxRateServiceTests
{
    private const string MunicipalityName = "Copenhagen";
    private static readonly DateTime Date = new(2024, 5, 2);

    private readonly IMunicipalityRepository _municipalityRepository = Substitute.For<IMunicipalityRepository>();
    private readonly ITaxRateRepository _taxRateRepository = Substitute.For<ITaxRateRepository>();
    private readonly TaxRateService _sut;

    public TaxRateServiceTests()
    {
        _sut = new TaxRateService(_municipalityRepository, _taxRateRepository);
    }

    private static TaxRate TaxRateOf(TaxType type, decimal rate, DateTime? createdAt = null)
    {
        return new TaxRate
        {
            Type = type,
            Rate = rate,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
    }

    private void GivenMunicipalityExists(long id = 1)
    {
        _municipalityRepository
            .GetMunicipalityByNameAsync(MunicipalityName, Arg.Any<CancellationToken>())
            .Returns(new Municipality { Id = id, Name = MunicipalityName });
    }

    private void GivenMunicipalityDoesNotExist()
    {
        _municipalityRepository
            .GetMunicipalityByNameAsync(MunicipalityName, Arg.Any<CancellationToken>())
            .Returns((Municipality?)null);
    }

    private void GivenValidTaxRates(params TaxRate[] taxRates)
    {
        _taxRateRepository
            .GetMunicipalityTaxRatesByDateAsync(Arg.Any<long>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns((IReadOnlyList<TaxRate>)taxRates);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsDaily_WhenDailyAndYearlyAreValid()
    {
        GivenMunicipalityExists();
        GivenValidTaxRates(TaxRateOf(TaxType.Yearly, 0.2m), TaxRateOf(TaxType.Daily, 0.1m));

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(TaxType.Daily, result.Value.Type);
        Assert.Equal(0.1m, result.Value.Rate);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsMonthly_WhenMonthlyAndYearlyAreValid()
    {
        GivenMunicipalityExists();
        GivenValidTaxRates(TaxRateOf(TaxType.Yearly, 0.2m), TaxRateOf(TaxType.Monthly, 0.4m));

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(TaxType.Monthly, result.Value.Type);
        Assert.Equal(0.4m, result.Value.Rate);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsTheOnlyValidRate()
    {
        GivenMunicipalityExists();
        GivenValidTaxRates(TaxRateOf(TaxType.Yearly, 0.2m));

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(0.2m, result.Value.Rate);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsMostRecent_WhenSameGranularityOverlaps()
    {
        GivenMunicipalityExists();
        var older = TaxRateOf(TaxType.Daily, 0.1m, new DateTime(2024, 1, 1));
        var newer = TaxRateOf(TaxType.Daily, 0.3m, new DateTime(2024, 2, 1));
        GivenValidTaxRates(older, newer);

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(0.3m, result.Value.Rate);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsNotFound_WhenMunicipalityDoesNotExist()
    {
        GivenMunicipalityDoesNotExist();

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task GetMunicipalityTaxRateByDate_ReturnsNotFound_WhenNoRateIsValid()
    {
        GivenMunicipalityExists();
        GivenValidTaxRates();

        var result = await _sut.GetMunicipalityTaxRateByDateAsync(MunicipalityName, Date, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task AddTaxRate_ReturnsNotFound_AndDoesNotPersist_WhenMunicipalityDoesNotExist()
    {
        GivenMunicipalityDoesNotExist();

        var result = await _sut.AddTaxRateAsync(MunicipalityName, TaxRateOf(TaxType.Yearly, 0.2m), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        await _taxRateRepository.DidNotReceive().AddTaxRateAsync(Arg.Any<TaxRate>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddTaxRate_SetsMunicipalityId_AndPersists_WhenMunicipalityExists()
    {
        GivenMunicipalityExists(id: 5);
        var taxRate = TaxRateOf(TaxType.Yearly, 0.2m);
        _taxRateRepository
            .AddTaxRateAsync(Arg.Any<TaxRate>(), Arg.Any<CancellationToken>())
            .Returns(taxRate);

        var result = await _sut.AddTaxRateAsync(MunicipalityName, taxRate, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.MunicipalityId);
        await _taxRateRepository.Received(1).AddTaxRateAsync(taxRate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateTaxRate_ReturnsSuccess_WhenARowIsUpdated()
    {
        _taxRateRepository
            .UpdateTaxRateAsync(1, Arg.Any<TaxRate>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _sut.UpdateTaxRateAsync(1, TaxRateOf(TaxType.Yearly, 0.2m), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateTaxRate_ReturnsNotFound_WhenNoRowIsUpdated()
    {
        _taxRateRepository
            .UpdateTaxRateAsync(Arg.Any<long>(), Arg.Any<TaxRate>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _sut.UpdateTaxRateAsync(99, TaxRateOf(TaxType.Yearly, 0.2m), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task AddTaxRate_ReturnsValidation_WhenStartDateIsAfterEndDate()
    {
        var taxRate = new TaxRate
        {
            Type = TaxType.Yearly,
            Rate = 0.2m,
            StartDate = new DateTime(2024, 12, 31),
            EndDate = new DateTime(2024, 1, 1)
        };

        var result = await _sut.AddTaxRateAsync(MunicipalityName, taxRate, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public async Task AddTaxRate_ReturnsValidation_WhenRateIsNegative()
    {
        var taxRate = new TaxRate
        {
            Type = TaxType.Yearly,
            Rate = -0.1m,
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 12, 31)
        };

        var result = await _sut.AddTaxRateAsync(MunicipalityName, taxRate, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public async Task UpdateTaxRate_ReturnsValidation_WhenStartDateIsAfterEndDate()
    {
        var taxRate = new TaxRate
        {
            Type = TaxType.Yearly,
            Rate = 0.2m,
            StartDate = new DateTime(2024, 12, 31),
            EndDate = new DateTime(2024, 1, 1)
        };

        var result = await _sut.UpdateTaxRateAsync(1, taxRate, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }
}
