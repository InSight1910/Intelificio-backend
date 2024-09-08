using Intelificio_Back.Common.Response;
using Intelificio_Back.Features.Authentication.Commands.Login;
using Intelificio_Back.Features.Authentication.Commands.Refresh;
using Intelificio_Back.Features.Authentication.Commands.Signup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intelificio_Back.Features.Authentication.Common
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (_) => Created(),
                onFailure: BadRequest);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<IActionResult>(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }
    }
}
