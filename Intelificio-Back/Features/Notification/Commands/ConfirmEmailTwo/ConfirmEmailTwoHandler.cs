using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Backend.Features.Notification.Commands.ConfirmEmailTwo
{
    public class ConfirmEmailTwoHandler(UserManager<User> userManager) : IRequestHandler<ConfirmEmailTwoCommand, Result>
    {
        public async Task<Result> Handle(ConfirmEmailTwoCommand request, CancellationToken cancellation)
        {
            var user = await userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(NotificationErrors.EmailNotSentOnConfirmEmailTwo);
            if (user.EmailConfirmed) return Result.Failure(NotificationErrors.UserAlreadyConfirmThisEmailOnConfirmEmailTwo);

            _ = await userManager.ConfirmEmailAsync(user, request.Token);
            return Result.Success();

        }
    }
}
