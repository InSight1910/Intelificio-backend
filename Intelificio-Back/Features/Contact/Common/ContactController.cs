using Backend.Common.Response;
using Backend.Features.Contact.Commands.Create;
using Backend.Features.Contact.Commands.Delete;
using Backend.Features.Contact.Commands.Update;
using Backend.Features.Contact.Queries.GetallByCommunity;
using Backend.Features.Contact.Queries.GetByID;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Contact.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IMediator mediator): ControllerBase
    {

      
        [HttpGet("GetAllByCommunity/{ID}")]
        public async Task<IActionResult> GetAllContactsByCommunityQuery(int ID)
        {
            var query = new GetAllContactsByCommunityQuery { CommunityId = ID };
            var Contact = await mediator.Send(query);
            return Contact.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var building = await mediator.Send(new GetContactByIdQuery { Id = ID });
            return building.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateContactCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(int ID, [FromBody] UpdateContactCommand command)
        {
            command.Id = ID;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete(int ID, [FromRoute] DeleteContactCommand command)
        {
            command.Id = ID;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (_) => Ok(),
                onFailure: BadRequest);
        }

       

    }
}
