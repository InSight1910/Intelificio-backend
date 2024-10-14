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

namespace Backend.Features.Notification.Commands.Package;

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
            .Include(u => u.Recipient)
            .ThenInclude(c => c.Communities)
            .ThenInclude(m => m.Municipality)
            .Where(p => p.ID == request.PackageID)
            .FirstOrDefaultAsync(cancellationToken);
        if (package == null) return Result.Failure(NotificationErrors.PackageNotFound);

        if (package.NotificacionSent < 3)
        {
            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(
                package.Recipient.Email ?? "intelificio@duocuc.cl",
                $"{package.Recipient.FirstName} {package.Recipient.LastName}"
            ));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "Package")
                .FirstOrDefaultAsync(cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnPackage);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId))
                return Result.Failure(NotificationErrors.TemplateIdIsNullOnPackage);

            var from = new EmailAddress("intelificio@duocuc.cl", package.Community.Name + " a través de Intelificio");

            var template = new PackageTemplate
            {
                CommunityName = package.Community.Name,
                Day = package.ReceptionDate.ToString("dd-MM-yyyy"),
                Hour = package.ReceptionDate.ToString("hh:mm tt", CultureInfo.InvariantCulture),
                SenderAddress = $"{package.Community.Address}{", "}{package.Community.Municipality.Name}",
                Name = package.Recipient.FirstName,
                TrackingNumber = package.TrackingNumber
            };

            var result =
                await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template, templateNotification.TemplateId,
                    from, recipients);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnPackage);

            package.NotificacionSent += 1;
            package.NotificationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time"));
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        else
        {
            return Result.Failure(NotificationErrors.LimmitNotificationSentOnPackage); 
        }


    }
}