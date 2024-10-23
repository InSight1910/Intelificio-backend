using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.MassUserConfirmationEmail;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Backend.Features.Notification.Commands.ConfirmEmailTwo
{
    public class MassUserConfirmationEmailHandler: IRequestHandler<MassUserConfirmationEmailCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<MassUserConfirmationEmailHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public MassUserConfirmationEmailHandler(IntelificioDbContext context,ILogger<MassUserConfirmationEmailHandler> logger, UserManager<User> userManager, IMapper mapper, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(MassUserConfirmationEmailCommand request,CancellationToken cancellationToken) 
        {

            var community = await _context.Community.Where(c => c.ID == request.CommunityID).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (community is null) return Result.Failure(NotificationErrors.CommunityNotfoundOnMassUserConfirmationEmail);

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ConfirmEmail").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnMassUserConfirmationEmail);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnMassUserConfirmationEmail);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{community.Name} {"a través de Intelificio"}");

            var templates = new List<MassUserConfirmationEmailTemplate>();
            var recipients = new List<EmailAddress>();

            foreach (var user in request.Users) 
            {

                var userEmail = await _userManager.FindByEmailAsync(user.Email);

                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                {
                    _logger.LogWarning("User is null or has an invalid email. Skipping user.");
                    continue;
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEmail);

                templates.Add(new MassUserConfirmationEmailTemplate
                {
                    UserName = user.ToString(),
                    ConfirmLink = $"http://localhost:4200/ConfirmarCorreo?email={user.Email}&token={WebUtility.UrlEncode(token)}",
                    CommunityName = community.Name

                });

                recipients.Add(new EmailAddress(user.Email, user.ToString()));
            }

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreatedOnMassUserConfirmationEmail);
            if (recipients == null) return Result.Failure(NotificationErrors.RecipientsNotCreatedOnMassUserConfirmationEmail);

            var result = await _sendMail.SendMassEmailConfirmationToMultipleRecipients(from, recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnMassUserConfirmationEmail);
            return Result.Success();
        }
    }
}
