using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Backend.Features.Authentication.Commands.ChangePasswordTwo
{
    public class ChangePasswordTwoCommandHandler : IRequestHandler<ChangePasswordTwoCommand, Result>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordTwoCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(ChangePasswordTwoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded) return Result.Success();

            var errors = result.Errors.Select(e => e.Description).ToList();
            return Result.Failure(AuthenticationErrors.InvalidParameters(errors));
        }
    }
}
