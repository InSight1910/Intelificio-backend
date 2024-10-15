using Backend.Common.Response;
using Backend.Features.Packages.Command;
using Backend.Features.Packages.Command.AssignCanRetire;
using Backend.Features.Packages.Command.Create;
using Backend.Features.Packages.Queries.GetByCommunity;
using Backend.Features.Packages.Queries.GetByUser;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Packages.Common;

[ApiController]
[Route("api/[controller]")]
public class PackageController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePackageCommand package)
    {
        var result = await mediator.Send(package);
        return result.Match(
            res => StatusCode(StatusCodes.Status201Created, res),
            err => BadRequest(err)
        );
    }

    [HttpGet("community/{id}")]
    public async Task<IActionResult> GetByCommunity(int id)
    {
        var result = await mediator.Send(new GetByCommunityQuery
        {
            CommunityId = id
        });

        return result.Match(
            res => Ok(res),
            err => BadRequest(err));
    }

    [HttpPut("markAsDelivered/{communityId}/{id}/{deliveredToId}")]
    public async Task<IActionResult> MarkAsDelivered(int id, int deliveredToId, int communityId)
    {
        var result = await mediator.Send(new MarkAsDeliveredCommand
            { Id = id, DeliveredToId = deliveredToId, CommunityId = communityId });
        return result.Match(
            res => Ok(res),
            err => BadRequest(err));
    }

    [HttpGet("[action]/{communityId}/{id}")]
    public async Task<IActionResult> GetMyPackages(int communityId, int id)
    {
        var result = await mediator.Send(new GetByUserQuery
        {
            CommunityId = communityId, UserId = id
        });
        return result.Match(
            res => Ok(res),
            err => BadRequest(err));
    }

    [HttpPut("[action]/{communityId}/{packageId}/{id}")]
    public async Task<IActionResult> AssignToRetire(int id, int communityId, int packageId)
    {
        var result = await mediator.Send(new AssignCanRetireCommand
        {
            CommunityId = communityId,
            PackageId = packageId,
            UserId = id
        });
        return result.Match(
            res => Ok(res),
            err => BadRequest(err));
    }
}