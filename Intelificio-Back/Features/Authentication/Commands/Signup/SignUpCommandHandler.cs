using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper) : IRequestHandler<SignUpCommand, Result>
    {
        public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            if (request.User != null)
            {
                return await DoSignUp(request.User);
            }
            else if (request.Users != null)
            {
                var tasks = new List<Task<Result>>();
                foreach (var user in request.Users)
                {
                    tasks.Add(DoSignUp(user));
                }
                var results = await Task.WhenAll(tasks);
                if (results.Any(r => r.IsFailure)) return Result.WithErrors(AuthenticationErrors.SignUpMassiveError(results.SelectMany(r => r.Errors).ToList()));
                return Result.Success();
            }
            return Result.Failure(null);
        }


        private async Task<Result> DoSignUp(UserObject request)
        {
            var userExist = await userManager.FindByEmailAsync(request.Email);
            if (userExist != null) return Result.Failure(AuthenticationErrors.AlreadyCreated);

            var user = mapper.Map<User>(request);

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
            var role = await roleManager.FindByNameAsync("Usuario");
            _ = await userManager.AddToRoleAsync(user, role!.Name!);

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            _ = await userManager.ConfirmEmailAsync(user, confirmationToken);
            return Result.Success();
        }
    }
}
