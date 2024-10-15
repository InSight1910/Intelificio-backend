using Backend.Common.Response;
using Backend.Features.Guest.Commands.Create;
using Backend.Features.Guest.Commands.Delete;
using Backend.Features.Guest.Commands.Update;
using Backend.Features.Guest.Queries.GetById;
using Backend.Features.Guest.Queries.GetAllByUnit;
using Backend.Features.Guest.Queries.GetAllByEntryTime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Backend.Features.Guest.Queries.GetAllByCommunity;

namespace Backend.Features.Guest.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateGuestCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id, [FromRoute] DeleteGuestCommand command)
        {
            command.Id = Id;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UpdateGuestCommand command)
        {
            command.Id = Id;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{Id}")]
        public async Task<IActionResult> GetByID(int Id)
        {
            var result = await mediator.Send(new GetByIdGuestQuery { Id = Id });
            return result.Match(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetAllByUnit/{unitId}")]
        public async Task<IActionResult> GetAllByUnit(int unitId)
        {
            var result = await mediator.Send(new GetAllByUnitGuestQuery { UnitId = unitId });
            return result.Match(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetAllByEntryTime")]
        public async Task<IActionResult> GetAllByEntryTime([FromQuery] DateTime entryTime)
        {
            var result = await mediator.Send(new GetAllByEntryTimeGuestQuery { EntryTime = entryTime});
            return result.Match(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetAllByCommunity/{communityId}")]
        public async Task<IActionResult> GetAllByCommunity(int communityId)
        {
            var result = await mediator.Send(new GetAllByCommunityGuestQuery { CommunityId = communityId });
            return result.Match(resultado => Ok(resultado), resultado => NotFound(resultado));
        }
    }
}
