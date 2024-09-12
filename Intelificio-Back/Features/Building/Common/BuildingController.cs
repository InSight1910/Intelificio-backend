using Backend.Common.Response;
using Backend.Features.Building.Commands.Create;
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
    }
}
