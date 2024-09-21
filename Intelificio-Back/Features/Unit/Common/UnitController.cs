using Backend.Common.Response;
using Backend.Features.Unit.Commands.Create;
using Backend.Features.Unit.Commands.Delete;
using Backend.Features.Unit.Commands.Update;
using Backend.Features.Unit.Queries.GetByID;
using Backend.Features.Unit.Queries.GetByUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Unit.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController(IMediator mediator) : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateUnitCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpDelete("{iD}")]
        public async Task<IActionResult> Delete(int iD, [FromRoute] DeleteUnitCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpPut("{iD}")]
        public async Task<IActionResult> Update(int iD, [FromBody] UpdateUnitCommand command)
        {
            command.Id = iD;
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await mediator.Send(new GetByIDQuery { UnitId = id });
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }

        [HttpGet("GetByUser/{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            var result = await mediator.Send(new GetByUserQuery { UserId = id });
            return result.Match<IActionResult>(resultado => Ok(resultado), resultado => NotFound(resultado));
        }
    }
}
