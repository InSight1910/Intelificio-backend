using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Net;

namespace Backend.Features.Authentication.Commands.ChangePasswordOne
{
    public class ChangePasswordOneCommandHandler : IRequestHandler<ChangePasswordOneCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;

        public ChangePasswordOneCommandHandler(IntelificioDbContext context, UserManager<User> userManager, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
        }

        public async Task<Result> Handle(ChangePasswordOneCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user == null) return Result.Failure(AuthenticationErrors.UserNotFound);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var template = new ChangePasswordTemplate
            {
                ResetLink = $"http://localhost:4200/change-password?email={user.Email}&token={WebUtility.UrlEncode(token)}",
                UserName = user.FirstName ?? ""
            };

            
            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ChangePassword").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(AuthenticationErrors.TemplateNotFoundOnChangePasswordOne);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(AuthenticationErrors.TemplateIdIsNullOnChangePasswordOne);

            var result = await _sendMail.SendSingleDynamicEmailToSingleRecipientAsync(user.Email!, template, templateNotification.TemplateId);

            if (!result.IsSuccessStatusCode) return Result.Failure(AuthenticationErrors.EmailNotSent);
            return Result.Success();
        }
    }
}
