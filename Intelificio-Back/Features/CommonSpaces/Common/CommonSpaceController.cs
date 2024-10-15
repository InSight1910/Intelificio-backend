using Backend.Common.Response;
using Backend.Features.CommonSpaces.Commands.Create;
using Backend.Features.CommonSpaces.Commands.Delete;
using Backend.Features.CommonSpaces.Commands.Update;
using Backend.Features.CommonSpaces.Queries.GetAllByCommunity;
using Backend.Features.CommonSpaces.Queries.GetById;
using Backend.Features.Notification.Commands.Maintenance;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.CommonSpaces.Common;

[Route("api/[controller]")]
[ApiController]
public class CommonSpaceController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCommonSpace([FromBody] CreateCommonSpaceCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Match(
            response => StatusCode(StatusCodes.Status201Created, response),
            error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCommonSpace([FromBody] UpdateCommonSpaceCommand command, int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
            if (command.IsInMaintenance)
            {
                var maintenanceCommand = new MaintenanceCommand
                {
                    CommunityID = command.CommunityId,
                    CommonSpaceID = command.Id,
                    StartDate = command.StartDate,
                    EndDate = command.EndDate,
                    Comment = command.Comment,
                    IsInMaintenance = command.IsInMaintenance
                };

                var maintenanceResult = await _mediator.Send(maintenanceCommand);
            }

        return result.Match(
            response => Ok(response),
            error => BadRequest(error)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommonSpace(int id)
    {
        var command = new DeleteCommonSpaceCommand { Id = id };
        var result = await _mediator.Send(command);
        return result.Match(
            response => Ok(response),
            error => BadRequest(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommonSpace(int id)
    {
        var command = new GetByIdCommonSpaceQuery { Id = id };
        var result = await _mediator.Send(command);
        return result.Match(
            response => Ok(response),
            error => NotFound(error)
        );
    }

    [HttpGet("community/{id}")]
    public async Task<IActionResult> GetCommonSpacesByCommunity(int id)
    {
        var command = new GetAllByCommunityQuery { CommunityId = id };
        var result = await _mediator.Send(command);
        return result.Match(
            response => Ok(response),
            error => NotFound(error)
        );
    }
}