using Backend.Common.Response;
using Backend.Features.Buildings.Commands.Create;
using Backend.Features.Buildings.Commands.Delete;
using Backend.Features.Buildings.Commands.Update;
using Backend.Features.Buildings.Queries.GetAllByCommunity;
using Backend.Features.Buildings.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Buildings.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateBuildingCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{iD}")]
        public async Task<IActionResult> Delete(int iD, [FromRoute] DeleteBuildingCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(int ID, [FromBody] UpdateBuildingCommand command)
        {
            command.Id = ID;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var building = await mediator.Send(new GetByIDQuery { BuildingId = ID });
            return building.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllByCommunity(int ID)
        {
            var query = new GetAllByCommunityQuery { CommunityId = ID };
            var buildings = await mediator.Send(query);
            return buildings.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

    }
}
