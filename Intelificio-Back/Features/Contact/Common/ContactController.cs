using Backend.Common.Response;
using Backend.Features.Contact.Commands.Create;
using Backend.Features.Contact.Queries.GetallByCommunity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Contact.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IMediator mediator): ControllerBase
    {

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateContactCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllContactsByCommunityQuery(int ID)
        {
            var query = new GetAllContactsByCommunityQuery { CommunityId = ID };
            var Contact = await mediator.Send(query);
            return Contact.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

    }
}
