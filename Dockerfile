# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files to preserve dependency restore caching
COPY ["MunicipalityTaxService.Api/MunicipalityTaxService.Api.csproj", "MunicipalityTaxService.Api/"]
COPY ["MunicipalityTaxService.Application/MunicipalityTaxService.Application.csproj", "MunicipalityTaxService.Application/"]
COPY ["MunicipalityTaxService.Domain/MunicipalityTaxService.Domain.csproj", "MunicipalityTaxService.Domain/"]
COPY ["MunicipalityTaxService.Infrastructure/MunicipalityTaxService.Infrastructure.csproj", "MunicipalityTaxService.Infrastructure/"]
COPY ["MunicipalityTaxService.Shared/MunicipalityTaxService.Shared.csproj", "MunicipalityTaxService.Shared/"]

# Restore dependencies
RUN dotnet restore "MunicipalityTaxService.Api/MunicipalityTaxService.Api.csproj"

# Copy remaining source code and build
COPY . .
WORKDIR "/src/MunicipalityTaxService.Api"
RUN dotnet build "MunicipalityTaxService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MunicipalityTaxService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final production-ready container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MunicipalityTaxService.Api.dll"]
