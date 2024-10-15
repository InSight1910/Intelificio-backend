using Backend.Common.Response;
using Backend.Features.Attendees.Commands.Create;
using Backend.Features.Attendees.Commands.Delete;
using Backend.Features.Attendees.Queries.GetAttendeeByReservation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Attendees.Common;

[ApiController]
[Route("api/[controller]")]
public class AttendeeController(IMediator mediator) : ControllerBase
{
    [HttpGet("reservation/{id}")]
    public async Task<IActionResult> GetAttendeesByReservation(int id)
    {
        var result = await mediator.Send(new GetAttendeeByReservationQuery { ReservationId = id });
        return result.Match(
            res => Ok(res),
            err => BadRequest(err));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAttendee([FromBody] CreateAttendeeCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(
            res => StatusCode(StatusCodes.Status201Created, res),
            err => BadRequest(err)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendee(int id)
    {
        var result = await mediator.Send(new DeleteAttendeeCommand { AttendeeId = id });
        return result.Match(
            res => Accepted(),
            err => BadRequest(err)
        );
    }
}