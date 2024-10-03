using Backend.Common.Response;
using Backend.Features.Reservations.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Reservations.Common;

[Route("api/[controller]")]
[ApiController]
public class ReservationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateReservationCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(
            res => StatusCode(StatusCodes.Status201Created, res),
            BadRequest
        );
    }
}