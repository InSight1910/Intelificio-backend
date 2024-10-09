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
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<ConfirmEmailHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ConfirmEmailHandler(IntelificioDbContext context, ILogger<ConfirmEmailHandler> logger, UserManager<User> userManager, IMapper mapper, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ConfirmEmail").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnConfirmEmail);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnConfirmEmail);


            var from = new EmailAddress("intelificio@duocuc.cl", "Intelificio");

            var templates = new List<ConfirmEmailTemplate>();
            var recipients = new List<EmailAddress>();

            foreach (var user in request.Users)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                templates.Add(new ConfirmEmailTemplate
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    ConfirmLink = $"http://localhost:4200/confirm-email?email={user.Email}&token={WebUtility.UrlEncode(token)}"
                });

                recipients.Add(new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"));
            }

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreatedOnConfirmEmail);
            if (recipients == null) return Result.Failure(NotificationErrors.RecipientsNotCreatedOnConfirmEmail);

            var result = await _sendMail.SendEmailConfirmationToMultipleRecipients(from, recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnConfirmEmail);
            return Result.Success();

            // Pendiente userManager.ConfirmEmailAsync(user, token); // recibo el token

        }
    }
}
