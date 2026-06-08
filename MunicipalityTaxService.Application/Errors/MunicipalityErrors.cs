using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Application.Errors;

public static class MunicipalityErrors
{
    public static Error NotFoundById(long id) =>
        Error.NotFound("Municipality.NotFound", $"No municipality with id '{id}' was found.");

    public static Error NotFoundByName(string name) =>
        Error.NotFound("Municipality.NotFound", $"No municipality named '{name}' was found.");

    public static Error AlreadyExists(string name) =>
        Error.Conflict("Municipality.AlreadyExists", $"A municipality named '{name}' already exists.");
}
