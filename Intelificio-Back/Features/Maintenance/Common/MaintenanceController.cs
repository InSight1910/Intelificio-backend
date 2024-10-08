using Backend.Common.Response;
using Backend.Features.Maintenance.Queries.GetAllByCommunity;
using Backend.Features.Maintenance.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Maintenance.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController(IMediator mediator) : ControllerBase
    {
        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllByCommunity(int ID)
        {
            var query = new GetAllByCommunityQuery { CommunityId = ID };
            var maintenance = await mediator.Send(query);
            return maintenance.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var maintenance = await mediator.Send(new GetByIdQuery { MaintenanceId = ID });
            return maintenance.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }
    }
}
