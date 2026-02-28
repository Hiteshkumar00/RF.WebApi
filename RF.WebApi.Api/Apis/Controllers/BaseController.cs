using Microsoft.AspNetCore.Mvc;
using RF.WebApi.Api.Domain.Exceptions;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    // This helper method lives here so all controllers can use it
    protected IActionResult HandleResponse<T>(ServiceResponse<T> response)
    {
        if (!response.Success)
        {
            return BadRequest(response.ToResult());
        }
        return Ok(response.ToResult());
    }
}
