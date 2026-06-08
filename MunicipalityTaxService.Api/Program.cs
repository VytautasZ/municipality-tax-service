using System.Text.Json.Serialization;
using MunicipalityTaxService.Api;
using MunicipalityTaxService.Application.Configuration;
using MunicipalityTaxService.Infrastructure.Configurations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.MapControllers();

await app.Services.ApplyMigrationsAsync();

app.Run();
