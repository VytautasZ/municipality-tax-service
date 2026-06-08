using MunicipalityTaxService.Application.Interfaces;
using MunicipalityTaxService.Application.Services;
using MunicipalityTaxService.Domain.Entities;
using MunicipalityTaxService.Shared;
using NSubstitute;

namespace MunicipalityTaxService.UnitTests;

public class MunicipalityServiceTests
{
    private readonly IMunicipalityRepository _municipalityRepository = Substitute.For<IMunicipalityRepository>();
    private readonly MunicipalityService _sut;

    public MunicipalityServiceTests()
    {
        _sut = new MunicipalityService(_municipalityRepository);
    }

    [Fact]
    public async Task GetMunicipalityById_ReturnsSuccess_WhenMunicipalityExists()
    {
        _municipalityRepository
            .GetMunicipalityByIdAsync(7, Arg.Any<CancellationToken>())
            .Returns(new Municipality { Id = 7, Name = "Copenhagen" });

        var result = await _sut.GetMunicipalityByIdAsync(7, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(7, result.Value.Id);
    }

    [Fact]
    public async Task GetMunicipalityById_ReturnsNotFound_WhenMunicipalityDoesNotExist()
    {
        _municipalityRepository
            .GetMunicipalityByIdAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((Municipality?)null);

        var result = await _sut.GetMunicipalityByIdAsync(99, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
    }

    [Fact]
    public async Task AddMunicipality_ReturnsSuccess_WithThePersistedMunicipality()
    {
        var municipality = new Municipality { Id = 3, Name = "Aarhus" };
        _municipalityRepository
            .AddMunicipalityAsync(municipality, Arg.Any<CancellationToken>())
            .Returns(municipality);

        var result = await _sut.AddMunicipalityAsync(municipality, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Id);
    }

    [Fact]
    public async Task AddMunicipality_ReturnsConflict_WhenNameAlreadyExists()
    {
        var municipality = new Municipality { Name = "Copenhagen" };
        _municipalityRepository
            .GetMunicipalityByNameAsync("Copenhagen", Arg.Any<CancellationToken>())
            .Returns(new Municipality { Id = 1, Name = "Copenhagen" });

        var result = await _sut.AddMunicipalityAsync(municipality, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
