# Municipality Tax Service

A small service for managing taxes applied in different municipalities. Tax rates are valid for specific
periods at one of four granularities (Daily, Weekly, Monthly, Yearly); given a municipality and a date, the
service returns the **single applicable rate**.

When several rates are valid on the same date, the **finest granularity wins**:

```
Daily  >  Weekly  >  Monthly  >  Yearly
```

Worked example (Copenhagen — this data is seeded on first run):

| Municipality | Date | Result | Winning rate |
|--------------|------|--------|--------------|
| Copenhagen | 2024-01-01 | **0.1** | Daily (overrides Yearly) |
| Copenhagen | 2024-03-16 | **0.2** | Yearly |
| Copenhagen | 2024-05-02 | **0.4** | Monthly (overrides Yearly) |
| Copenhagen | 2024-07-10 | **0.2** | Yearly |

## Technologies

- **.NET 10** / **ASP.NET Core Web API** (attribute-routed controllers)
- **Entity Framework Core 10** with **SQL Server**
- **.NET Aspire 9** — local orchestration (`AppHost`) + `ServiceDefaults` (OpenTelemetry, health checks, service discovery)
- **Docker** / **Docker Compose** — containerised API + SQL Server
- **Scalar** — OpenAPI reference UI (replaces Swagger UI)
- **xUnit** + **NSubstitute** — unit tests
- **Clean Architecture**, the **Result pattern**, and **RFC 7807 ProblemDetails** for graceful error handling

## Solution structure

| Project | Responsibility |
|---------|----------------|
| `MunicipalityTaxService.Api` | Controllers, DTOs, mappers, global exception handler, composition root |
| `MunicipalityTaxService.Application` | Services, service/repository interfaces, error definitions |
| `MunicipalityTaxService.Domain` | Entities (`Municipality`, `TaxRate`) and the `TaxType` enum |
| `MunicipalityTaxService.Infrastructure` | EF Core `DbContext`, entity configurations, repositories, migrations, **seeder** |
| `MunicipalityTaxService.Shared` | Shared kernel — `OperationResult`, `Error`, `ErrorType` |
| `MunicipalityTaxService.AppHost` | .NET Aspire orchestrator |
| `MunicipalityTaxService.ServiceDefaults` | Telemetry, health checks, service discovery defaults |
| `tests/MunicipalityTaxService.UnitTests` | Unit tests (precedence rule, validation, services) |

Dependency flow is inward only: `Api → Application → Domain`, `Infrastructure → Application/Domain`, and all layers may use `Shared`.

## API

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/municipalities` | Add a municipality |
| `GET` | `/api/municipalities/{id}` | Get a municipality by id |
| `POST` | `/api/municipalities/{name}/tax-rates` | Add a tax rate for a municipality |
| `PUT` | `/api/municipalities/{name}/tax-rates/{id}` | Update a tax rate |
| `GET` | `/api/municipalities/{name}/tax-rates?date=yyyy-MM-dd` | Get the applicable tax rate on a date |

- Interactive API reference (**Scalar**): `/scalar/v1`
- OpenAPI document: `/openapi/v1.json`
- Health checks (Development): `/health`, `/alive`

## Database seeding

On startup the application applies EF Core migrations and then runs a **seeder** that inserts the **Copenhagen**
example data from the task (a yearly, a monthly, and two single-day daily rates). The seeder is **idempotent and
first-run only** — it inserts the data only when the database has no municipalities, and is a no-op on every
subsequent start. See [`MunicipalityTaxService.Infrastructure/DatabaseSeeder.cs`](MunicipalityTaxService.Infrastructure/DatabaseSeeder.cs).

## Running the application

Both options below start SQL Server in a container, apply migrations, and seed the example data automatically.

### Option 1 — .NET Aspire (recommended for development)

Requires **Docker Desktop** running.

```bash
dotnet run --project MunicipalityTaxService.AppHost
```

This launches the **Aspire dashboard**, which starts a SQL Server container and the API (the API `WaitFor`s SQL
to be healthy before migrating). Open the dashboard, click the **`api`** resource, and append `/scalar/v1` to reach
the API reference. SQL Server is exposed on a **random host port** — find it on the `sql` resource in the dashboard
(handy for connecting with SSMS).

### Option 2 — Docker Compose

Requires **Docker** (Compose v2).

```bash
docker compose up --build
```

| Service | URL / Endpoint |
|---------|----------------|
| API | <http://localhost:5005> |
| Scalar UI | <http://localhost:5005/scalar/v1> |
| SQL Server | `localhost,1445` (user `sa`, password `@test123`) |

Stop and remove everything (including the database volume):

```bash
docker compose down -v
```

### Option 3 — Run the API directly (no containers)

Requires a reachable SQL Server matching `ConnectionStrings:MunicipalityTaxDbString` in
`MunicipalityTaxService.Api/appsettings.json`.

```bash
dotnet run --project MunicipalityTaxService.Api
```

## Tests

```bash
dotnet test
```

## Error handling

- **Expected failures** (not found, validation, conflict) flow through the **`OperationResult`** pattern and are
  translated to **RFC 7807 ProblemDetails** with the appropriate status code (404 / 400 / 409).
- **Unexpected exceptions** are caught by a global `IExceptionHandler` that logs the real error and returns a
  generic `500` — internal details are never exposed to the caller.

## Development notes

### Commit convention prompt

[`.github/prompts/create-commit.prompt.md`](.github/prompts/create-commit.prompt.md) is a reusable AI prompt file
that documents this repository's commit/branch/PR convention used throughout development:

- Branch: `feature/VZ-<NN>_<Short_Title>`
- Commit subject: `VZ-<NN> <imperative verb> <what>` (verbs: **Add / Change / Remove / Fix**)
- Commit body: past-tense bullet points describing what was done

### Database migrations

Migrations live in `MunicipalityTaxService.Infrastructure/Migrations` and are applied automatically at startup.
To add a new migration:

```bash
dotnet ef migrations add <Name> -p MunicipalityTaxService.Infrastructure -s MunicipalityTaxService.Api
```
