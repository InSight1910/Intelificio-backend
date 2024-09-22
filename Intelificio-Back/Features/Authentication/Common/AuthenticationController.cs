using Backend.Common.Response;
using Backend.Features.Authentication.Commands.Login;
using Backend.Features.Authentication.Commands.Refresh;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Queries.GetUserByEmail;
using Backend.Features.Authentication.Queries.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Authentication.Common
{
    [Route("api/auth")]
    [ApiController]
    public class CommunityController(IMediator mediator) : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: (errors) =>
                {
                    return BadRequest(errors);
                });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: (errors) =>
                {
                    return BadRequest(errors);
                }
                );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpPost("user/byEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] GetUserByEmailQuery query)
        {
            var result = await mediator.Send(query);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await mediator.Send(new GetAllRolesQuery());
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
                
        }
    }
}
