using Backend.Common.Response;
using Backend.Features.Community.Queries.GetAllByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Community.Common
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CommunityController(IMediator mediator) : ControllerBase
    {
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var query = new GetAllByUserQuery { UserId = userId };

            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }
    }
}
