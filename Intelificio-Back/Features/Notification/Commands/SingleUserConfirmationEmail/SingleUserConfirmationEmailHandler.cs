using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Backend.Features.Notification.Commands.ConfirmEmail
{
    public class SingleUserConfirmationEmailHandler : IRequestHandler<SingleUserConfirmationEmailCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SingleUserConfirmationEmailHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public SingleUserConfirmationEmailHandler(IntelificioDbContext context, ILogger<SingleUserConfirmationEmailHandler> logger, UserManager<User> userManager, IMapper mapper, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(SingleUserConfirmationEmailCommand request, CancellationToken cancellationToken)
        {

            var community = await _context.Community.Where(c => c.ID == request.CommunityID).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (community is null) return Result.Failure(NotificationErrors.CommunityNotfoundOnSingleUserConfirmationEmail);

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ConfirmEmail").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnSingleUserConfirmationEmail);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnSingleUserConfirmationEmail);


            var from = new EmailAddress("intelificio@duocuc.cl", $"{community.Name} {"a través de Intelificio"}");

            var templates = new List<SingleUserConfirmationEmailTemplate>();
            var recipients = new List<EmailAddress>();

            foreach (var user in request.Users)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                templates.Add(new SingleUserConfirmationEmailTemplate
                {
                    UserName = user.ToString(),
                    ConfirmLink = $"http://localhost:4200/ConfirmarCorreo?email={user.Email}&token={WebUtility.UrlEncode(token)}",
                    CommunityName = community.Name

                });

                recipients.Add(new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"));
            }

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreatedOnSingleUserConfirmationEmail);
            if (recipients == null) return Result.Failure(NotificationErrors.RecipientsNotCreatedOnSingleUserConfirmationEmail);

            var result = await _sendMail.SendSingleEmailConfirmationToMultipleRecipients(from, recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnSingleUserConfirmationEmail);
            return Result.Success();

        }
    }
}
