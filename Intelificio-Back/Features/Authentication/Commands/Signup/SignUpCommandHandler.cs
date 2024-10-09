using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Commands.ConfirmEmail;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IMediator mediator) : IRequestHandler<SignUpCommand, Result>
    {
        public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            if (request.User != null)
            {
                return await DoSignUp(request.User);
            }
            else if (request.Users != null)
            {
                var results = new List<Result>();
                foreach (var user in request.Users)
                {
                    results.Add(await DoSignUp(user));
                }

                if (results.Any(r => r.IsFailure)) return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(results.Select(r => r.Error).ToList()));
                return Result.Success();
            }
            return Result.Failure(null);
        }


        private async Task<Result> DoSignUp(UserObject request)
        {
            var userExist = await userManager.FindByEmailAsync(request.Email);


            if (userExist != null) return Result.Failure(AuthenticationErrors.AlreadyCreatedEmail(request.Email));

            var user = mapper.Map<User>(request);

            var roleExist = await roleManager.FindByNameAsync(request.Role);
            if (roleExist == null) return Result.Failure(AuthenticationErrors.RoleNotFound);

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Errors.Any())
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return Result.Failure(AuthenticationErrors.InvalidParameters(errors));
            }

            _ = await userManager.AddToRoleAsync(user, roleExist.Name!);

            var confirmEmailCommand = new ConfirmEmailCommand
            {
                Users = new List<User> { user } 
            };

            var confirmEmailResult = await mediator.Send(confirmEmailCommand);

            if (confirmEmailResult.IsFailure)
            {
                return Result.Failure(confirmEmailResult.Errors);
            }

            // var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            // _ = await userManager.ConfirmEmailAsync(user, confirmationToken);
            return Result.Success();
        }
    }
}
