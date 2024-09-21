using Backend.Common.Response;
using Backend.Common.Security;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, TokenProvider tokenProvider, IConfiguration configuration) : IRequestHandler<LoginCommand, Result>
    {
        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(request.Email);

            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            if (!await userManager.IsEmailConfirmedAsync(user)) return Result.Failure(AuthenticationErrors.EmailNotConfirmed);

            if (await userManager.IsLockedOutAsync(user)) return Result.Failure(AuthenticationErrors.UserBlocked);

            var result = await signInManager.PasswordSignInAsync(user, request.Password, false, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    _ = await userManager.AccessFailedAsync(user);
                    return Result.Failure(AuthenticationErrors.UserBlocked);
                }
                return Result.Failure(AuthenticationErrors.WrongPassword);
            }

            var role = await userManager.GetRolesAsync(user);

            string token = tokenProvider.CreateToken(user, role.FirstOrDefault());
            string refreshToken = tokenProvider.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("Jwt:RefreshTokenExpireInMinutes"));
            _ = await userManager.UpdateAsync(user);
            var response = new ResponseData
            {
                Data = new LoginCommandResponse
                {
                    Token = token,
                    RefreshToken = refreshToken
                }
            };
            return Result.WithResponse(response);
        }
    }
}
