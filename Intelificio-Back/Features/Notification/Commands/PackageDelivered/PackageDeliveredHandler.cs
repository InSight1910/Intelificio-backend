using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Package;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.Notification.Commands.PackageDelivered
{
    public class PackageDeliveredHandler : IRequestHandler<PackageDeliveredCommand,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<PackageDeliveredHandler> _logger;

        public PackageDeliveredHandler(IntelificioDbContext context, ILogger<PackageDeliveredHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(PackageDeliveredCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Package
            .Include(u => u.Recipient)
            .ThenInclude(c => c.Communities)
            .ThenInclude(m => m.Municipality)
            .Where(p => p.ID == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
            if (package == null) return Result.Failure(NotificationErrors.PackageNotFoundOnPackageDelivered);

            var recipients = new List<EmailAddress>();
            var veliveratedto = "";
            if (package.RecipientId == request.DeliveredToId)
            {
                veliveratedto = package.Recipient.ToString();
                recipients.Add(new EmailAddress(package.Recipient.Email ?? "intelificio@duocuc.cl",package.Recipient.ToString()));
            } else
            {
                var a = await _context.Users.Where(x => x.Id == request.DeliveredToId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (a is null) return Result.Failure(NotificationErrors.DeliveredUserNotfoundOnPackageDelivered);
                veliveratedto = a.ToString();
                recipients.Add(new EmailAddress(a.Email, veliveratedto));
            }

            recipients.Add(new EmailAddress(
                package.Recipient.Email ?? "intelificio@duocuc.cl",
                package.Recipient.ToString()
            ));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "PackageDelivered")
                .FirstOrDefaultAsync(cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnPackageDelivered);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId))
                return Result.Failure(NotificationErrors.TemplateIdIsNullOnPackageDelivered);

            var from = new EmailAddress("intelificio@duocuc.cl", package.Community.Name + " a través de Intelificio");

            var templates = new List<PackageDeliveredTemplate>();
            var template = new PackageDeliveredTemplate
            {
                CommunityName = package.Community.Name,
                Day = package.DeliveredDate?.ToString("dd-MM-yyyy") ?? "Fecha no disponible",
                Hour = package.DeliveredDate?.ToString("hh:mm tt", CultureInfo.InvariantCulture) ?? "Hora no disponible",
                SenderAddress = $"{package.Community.Address}{", "}{package.Community.Municipality.Name}",
                Name = package.Recipient.ToString(),
                TrackingNumber = package.TrackingNumber,
                Deliveratedto = veliveratedto,
            };
            templates.Add(template);

            var result =
                await _sendMail.SendPackageDeliveredNotification(from,recipients,templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnPackageDelivered);

            return Result.Success();
        }

    }
}
