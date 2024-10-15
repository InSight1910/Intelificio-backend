using Backend.Common.Response;
using Backend.Features.Community.Commands.AddUser;
using Backend.Features.Community.Commands.AddUserMassive;
using Backend.Features.Community.Commands.Create;
using Backend.Features.Community.Commands.Delete;
using Backend.Features.Community.Commands.RemoveUser;
using Backend.Features.Community.Commands.Update;
using Backend.Features.Community.Queries.GetAll;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Features.Community.Queries.GetById;
using Backend.Features.Community.Queries.GetUsersByCommunity;
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
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("{communityId}/users")]
        public async Task<IActionResult> GetUsersByCommunity(int communityId)
        {
            var query = new GetUsersByCommunityQuery { CommunityId = communityId };

            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllCommunitiesQuery { });
            return Ok(result.Response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetByIdCommunityQuery { Id = id });
            return result.Match(
                onSuccess: response => Ok(response),
                onFailure: NotFound
                );
        }

        [HttpPut("add/{id}/{userId}")]
        public async Task<IActionResult> AddUserToCommunity(int id, int userId)
        {
            var result = await mediator.Send(new AddUserCommunityCommand
            {
                User = new AddUserObject
                {
                    CommunityId = id,
                    UserId = userId
                }
            });
            return result.Match(
                onSuccess: response => Ok(response),
                onFailure: error => NotFound(error)
                );
        }

        [HttpPut("remove/{id}/{userId}")]
        public async Task<IActionResult> RemoveUserToCommunity(int id, int userId)
        {
            var result = await mediator.Send(new RemoveUserCommunityCommand { CommunityId = id, UserId = userId });
            return result.Match(
                onSuccess: response => Ok(response),
                onFailure: NotFound
                );
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommunityCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: result => Ok(result),
                onFailure: (result) => BadRequest(result)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteCommunityCommand { Id = id });
            return result.Match(
                onSuccess: _ => Ok(),
                onFailure: BadRequest
                );
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommunityCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: response => Ok(response),
                onFailure: BadRequest
                );
        }

        [HttpPost("add/user/massive")]
        public IActionResult SignUpMassive([FromForm] AddUserMassiveCommand command, [FromServices] IServiceScopeFactory serviceScopeFactory)
        {

            var memoryStream = new MemoryStream();
            command.File.CopyTo(memoryStream);
            memoryStream.Position = 0;
            command.Stream = memoryStream;

            _ = Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        // Resolve IMediator inside the new scope
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(command);
                        // TODO: Enviar mensaje informando resultado

                    }
                    catch (Exception ex)
                    {
                        // Log any exceptions that might occur
                        Console.WriteLine("Exception: " + ex.Message);
                    }
                    return Task.CompletedTask;
                }
            });
            return Accepted();
        }


    }
}
