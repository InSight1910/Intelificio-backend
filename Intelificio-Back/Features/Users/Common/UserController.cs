using Backend.Common.Response;
using Backend.Features.Users.Queries.GetAllByCommunity;
using Backend.Features.Users.Queries.GetByRut;
using Backend.Features.Users.Queries.GetConcierges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Users.Queries.Common;

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

    [HttpGet("[action]/{id}/{rut}")]
    public async Task<IActionResult> GetByRutCommunity(int id, string rut)
    {
        var result = await mediator.Send(new GetByRutQuery { Rut = rut, CommunityId = id });
        return result.Match(
            user => Ok(user),
            error => NotFound(error));
    }

    [HttpGet("concierges/{communityId}")]
    public async Task<IActionResult> GetConciergesByCommunityId(int communityId)
    {
        var result = await mediator.Send(new GetConciergesQuery { CommunityId = communityId });
        return result.Match(
            user => Ok(user),
            error => BadRequest(error));
    }

    [HttpGet("GetAllByCommunity/{communityId}")]
    public async Task<IActionResult> GetAllByCommunity(int communityId)
    {
        var query = new GetAllByCommunityUsersQuery { CommunityId = communityId };
        var buildings = await mediator.Send(query);
        return buildings.Match(
            onSuccess: (response) => Ok(response),
            onFailure: BadRequest);
    }
}