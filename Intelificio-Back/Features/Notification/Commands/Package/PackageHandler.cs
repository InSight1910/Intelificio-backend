using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.SingleMessage;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Globalization;

namespace Backend.Features.Notification.Commands.Package
{
    public class PackageHandler : IRequestHandler<PackageCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<PackageHandler> _logger;

        public PackageHandler(IntelificioDbContext context, ILogger<PackageHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(PackageCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Package
                .Include(u => u.Owner)
                .ThenInclude(c => c.Communities)
                .ThenInclude(m => m.Municipality)
                .Where(p => p.ID == request.PackageID)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (package == null) return Result.Failure(NotificationErrors.PackageNotFound);

            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(
                package.Owner.Email ?? "intelificio@duocuc.cl",
                $"{package.Owner.FirstName} {package.Owner.LastName}"
            ));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "Package").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnPackage);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnPackage);

            var from = new EmailAddress("intelificio@duocuc.cl", package.Community.Name + " a través de Intelificio");

            var template = new PackageTemplate
            {
                CommunityName = package.Community.Name,
                Day = package.ReceptionDate.ToString("dd-MM-yyyy"),
                Hour = package.ReceptionDate.ToString("hh:mm tt",CultureInfo.InvariantCulture),
                SenderAddress = $"{package.Community.Address}{", "}{package.Community.Municipality.Name}",
                Name = package.Owner.FirstName,
            };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template, templateNotification.TemplateId, from, recipients);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnPackage);
            return Result.Success();
        }    
    }
}
