using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailUserCommandHandler : IRequestHandler<ConfirmEmailUserCommand, Result>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(ConfirmEmailUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (result.Succeeded) return Result.Success();
            return Result.Failure(AuthenticationErrors.InvalidToken);
        }
    }
}