using Intelificio_Back.Common.Response;
using Intelificio_Back.Common.Security;
using Intelificio_Back.Features.Authentication.Common;
using Intelificio_Back.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Intelificio_Back.Features.Authentication.Commands.Refresh
{
    public class RefreshCommandHandler(UserManager<User> userManager, TokenProvider tokenProvider, IConfiguration configuration) : IRequestHandler<RefreshCommand, Result>
    {
        public async Task<Result> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var principals = tokenProvider.GetPrincipalFromExpiredToken(request.Token);

            var user = await userManager.FindByNameAsync(principals.Claims.Where(e => e.Type.Equals(ClaimTypes.Email)).First().Value);
            var invalidRefreshToken = false;
            var invalidRefreshTokenErrors = new List<string>();

            if (!user.RefreshToken.Equals(request.RefreshToken))
            {
                invalidRefreshTokenErrors.Add("Different Refresh Token");
                invalidRefreshToken = true;
            }

            if (user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                invalidRefreshTokenErrors.Add("Refresh Token Expired");
                invalidRefreshToken = true;
            }

            if (invalidRefreshToken)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                var responseRevokeRefresh = await userManager.UpdateAsync(user);
                if (responseRevokeRefresh.Succeeded)
                {
                    return Result.Failure(AuthenticationErrors.RefreshTokenError(invalidRefreshTokenErrors));
                }
                else
                {
                    var errorList = new List<string>();
                    foreach (var error in responseRevokeRefresh.Errors)
                        errorList.Add(error.Description);
                    return Result.Failure(AuthenticationErrors.RefreshTokenError(errorList));
                }
            }


            var newToken = tokenProvider.CreateToken(user);
            var refreshToken = tokenProvider.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("Jwt:RefreshTokenExpireInMinutes"));
            var response = await userManager.UpdateAsync(user);
            if (response.Succeeded)
            {
                return Result.SuccessWithResponse(new ResponseData
                {
                    Data = new RefreshCommandResponse
                    {
                        Token = newToken,
                        RefreshToken = refreshToken
                    }
                });
            }
            else
            {
                var errorList = new List<string>();
                foreach (var error in response.Errors)
                    errorList.Add(error.Description);
                return Result.Failure(AuthenticationErrors.RefreshTokenError(errorList));
            }
        }
    }
}
