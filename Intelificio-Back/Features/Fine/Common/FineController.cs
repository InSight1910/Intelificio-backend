using Backend.Common.Response;
using Backend.Features.Attendees.Commands.Delete;
using Backend.Features.Fine.Commands.Create;
using Backend.Features.Fine.Commands.Delete;
using Backend.Features.Fine.Commands.Update;
using Backend.Features.Fine.Queries.GetAllByCommunity;
using Backend.Features.Fine.Queries.GetFineById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Fine.Common
{
    [Route("api/[controller]")]
    public class FineController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> CreateFine([FromBody] CreateFineCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFine(int Id, [FromRoute] DeleteFineCommand command)
        {
            command.FineId = Id;
            var result = await mediator.Send(command);
            return result.Match(
            onSuccess: (_) => Ok(),
            onFailure: BadRequest);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateFine(int Id, [FromBody] UpdateFineCommand command)
        {
            command.FineId = Id;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByCommunity/{Id}")]
        public async Task<IActionResult> GetAllFineByCommunityId(int Id)
        {
            var fines = await mediator.Send(new GetAllFinesByCommunityQuery { CommunityId = Id });
            return fines.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetFineByID(int Id)
        {
            var fine = await mediator.Send(new GetFineByIdQuery { FineId = Id });
            return fine.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);

        }

    }
}
