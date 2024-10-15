using Backend.Common.Response;
using Backend.Common.Security;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Backend.Features.Authentication.Commands.UpdateUser
{
    public class UpdateUserHandler(UserManager<User> userManager, TokenProvider tokenProvider, IConfiguration configuration) : IRequestHandler<UpdateUserCommand, Result>
    {
        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

            // Update user properties with new values from request
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            // Save updated user info
            var updateUserResponse = await userManager.UpdateAsync(user);
            if (!updateUserResponse.Succeeded)
            {
                var errorList = new List<string>();
                foreach (var error in updateUserResponse.Errors)
                    errorList.Add(error.Description);
                return Result.Failure(AuthenticationErrors.UpdateUserError(errorList));
            }

            // Generate new tokens
            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            var newToken = tokenProvider.CreateToken(user, role);
            var refreshToken = tokenProvider.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("Jwt:RefreshTokenExpireInMinutes"));
            var response = await userManager.UpdateAsync(user);
            if (response.Succeeded)
            {
                return Result.WithResponse(new ResponseData
                {
                    Data = new UpdateUserResponse
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
