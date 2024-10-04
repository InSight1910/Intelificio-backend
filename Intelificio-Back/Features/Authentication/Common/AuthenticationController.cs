﻿using Backend.Common.Response;
using Backend.Features.Authentication.Commands.ChangePasswordOne;
using Backend.Features.Authentication.Commands.ChangePasswordTwo;
using Backend.Features.Authentication.Commands.Login;
using Backend.Features.Authentication.Commands.Refresh;
using Backend.Features.Authentication.Commands.Signup;
using Backend.Features.Authentication.Commands.SignupMassive;
using Backend.Features.Authentication.Commands.UpdateUser;
using Backend.Features.Authentication.Queries.GetAllRoles;
using Backend.Features.Authentication.Queries.GetAllUserAdmin;
using Backend.Features.Authentication.Queries.GetUserByEmail;
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
            return result.Match(
                onSuccess: (_) => Created(),
                onFailure: (errors) =>
                {
                    return BadRequest(errors);
                });
        }

        [HttpPost("signup/massive")]
        public IActionResult SignUpMassive([FromForm] SignupMassiveCommand command, [FromServices] IServiceScopeFactory serviceScopeFactory)
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

                        // Handle the result in the background

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

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
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
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpPut("user/update")]
        public async Task<IActionResult> Refresh([FromBody] UpdateUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpPost("user/byEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] GetUserByEmailQuery query)
        {
            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await mediator.Send(new GetAllRolesQuery());
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);

        }

        [HttpGet("User/admin")]
        public async Task<IActionResult> GetAllUserAdmin()
        {
            var result = await mediator.Send(new GetAllUserAdminQuery());
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: NotFound);

        }

        [HttpPost("change-password-one")]
        public async Task<IActionResult> ChangePasswordStepOne([FromBody] ChangePasswordOneCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }

        [HttpPost("change-password-two")]
        public async Task<IActionResult> ChangePasswordStepTwo([FromBody] ChangePasswordTwoCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: (response) => Ok(response),
                onFailure: BadRequest);
        }
    }
}
