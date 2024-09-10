using Backend.Common.Response;
using Backend.Common.Security;
using Backend.Features.Authentication.Common;
using Backend.Models;
using Intelificio_Back.Common.Response;
using Intelificio_Back.Features.Authentication.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler(UserManager<User> userManager, TokenProvider tokenProvider, IConfiguration configuration) : IRequestHandler<LoginCommand, Result>
    {
        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.Email);

            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            if (!user.EmailConfirmed) return Result.Failure(AuthenticationErrors.EmailNotConfirmed);

            if (user.LockoutEnabled) return Result.Failure(AuthenticationErrors.UserBlocked);

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                var result = await userManager.AccessFailedAsync(user);
                return ValidateResponse(result);
            }

            string token = tokenProvider.CreateToken(user);
            string refreshToken = tokenProvider.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddMinutes(configuration.GetValue<double>("Jwt:RefreshTokenExpireInMinutes"));
            _ = await userManager.UpdateAsync(user);
            var response = new ResponseData
            {
                Data = new LoginCommandResponse
                {
                    Token = token,
                    RefreshToken = refreshToken
                }
            };
            return Result.SuccessWithResponse(response);
        }
        private static Result ValidateResponse(IdentityResult result)
        {
            if (result.Succeeded)
            {
                return Result.Failure(AuthenticationErrors.InvalidCredentials(new List<string> { "Invalid Password" }));
            }
            else
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return Result.Failure(AuthenticationErrors.InvalidCredentials(errors));
            }
        }
    }
}
