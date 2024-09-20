using Backend.Common.Response;
using Backend.Features.Building.Commands.AddUnit;
using Backend.Features.Building.Commands.Create;
using Backend.Features.Building.Commands.Delete;
using Backend.Features.Building.Commands.RemoveUnit;
using Backend.Features.Building.Commands.Update;
using Backend.Features.Building.Queries.GetAllByCommunity;
using Backend.Features.Building.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Building.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateBuildingCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{iD}")]
        public async Task<IActionResult> Delete(int iD, [FromRoute] DeleteBuildingCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{iD}")]
        public async Task<IActionResult> Update(int iD, [FromBody] UpdateBuildingCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var building = await mediator.Send(new GetByIDQuery { BuildingId = ID });
            return building.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllByCommunity(int ID)
        {
            var query = new GetAllByCommunityQuery { CommunityId = ID };
            var buildings = await mediator.Send(query);
            return buildings.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

    }
}
