using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxService.Shared;

namespace MunicipalityTaxService.Api.Controllers;

/// <summary>
/// Base controller that maps application <see cref="Error"/> values to RFC 7807 problem responses.
/// </summary>
public abstract class ApiController : ControllerBase
{
    protected ObjectResult Problem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Description);
    }
}
