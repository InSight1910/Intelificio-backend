using Backend.Common.Response;
using Backend.Features.Users.GetByRut;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Users.Common;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("[action]/{rut}")]
    public async Task<IActionResult> GetByRut(string rut)
    {
        var result = await mediator.Send(new GetByRutQuery { Rut = rut });
        return result.Match(
            user => Ok(user),
            error => NotFound(error));
    }
}