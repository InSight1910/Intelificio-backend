using Backend.Common.Response;
using Backend.Features.Reservations.Commands;
using Backend.Features.Reservations.Commands.Create;
using Backend.Features.Reservations.Query;
using Backend.Features.Reservations.Query.GetReservationsByCommunityAndMonth;
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

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmReservationCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(
            _ => Accepted(),
            BadRequest
        );
    }

    [HttpGet("community/{id}/{date}")]
    public async Task<IActionResult> GetByCommunityAndMonth(int id, DateTime date)
    {
        var result = await mediator.Send(new GetReservationsByCommunityAndMonthQuery
            { CommunityId = id, Date = date });
        return result.Match(
            res => Ok(res),
            BadRequest);
    }

    [HttpGet("count/{id}/{year}/{month}")]
    public async Task<IActionResult> GetByCommunityAndDate(int id, int month, int year)
    {
        var result = await mediator.Send(new GetCountOfReservationByCommunityAndDateQuery
            { Month = month, Year = year, CommunityId = id });
        return result.Match(
            res => Ok(res),
            BadRequest);
    }
}