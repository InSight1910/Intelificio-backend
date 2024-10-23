using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.Notification.Commands.FineNotification
{
    public class FineNotificationHandler(IntelificioDbContext context, ILogger<FineNotificationHandler> logger, SendMail sendMail, UserManager<User> manager) : IRequestHandler<FineNotificationCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<FineNotificationHandler> _logger = logger;
        private readonly SendMail _sendMail = sendMail;
        private readonly UserManager<User> _userManager = manager;

        public async Task<Result> Handle(FineNotificationCommand request, CancellationToken cancellationToken)
        {

            var assignatedFine = await _context.AssignedFines
                .Include(x => x.Fine)
                .ThenInclude(x => x.Community)
                .ThenInclude(x => x.Municipality)
                .Include(x => x.Unit)
                .ThenInclude(x => x.Users)
                .FirstOrDefaultAsync(x => x.ID == request.AssignedFineId, cancellationToken);
            if (assignatedFine is null) return Result.Failure(NotificationErrors.AssignatedFineNotFoundOnFineNotification);


            var usersInUnit = assignatedFine.Unit.Users.ToList();

            var renters = await _userManager.GetUsersInRoleAsync("Arrendatario");
            var unitRenters = usersInUnit.Where(u => renters.Any(r => r.Id == u.Id)).ToList();

            List<EmailAddress> recipients = [];

            
            if (unitRenters.Count != 0)
            {
                recipients.AddRange(unitRenters.Select(renter => new EmailAddress(renter.Email, renter.ToString())));

                var owners = await _userManager.GetUsersInRoleAsync("Propietario");
                var unitOwners = usersInUnit.Where(u => owners.Any(o => o.Id == u.Id)).ToList();
                if (unitOwners.Count != 0)
                {
                    recipients.AddRange(unitOwners.Select(owner => new EmailAddress(owner.Email, owner.ToString())));
                }
                else
                {
                    return Result.Failure(NotificationErrors.NoOwnersFoundOnFineNotification);
                }
            }
            else
            {
                var owners = await _userManager.GetUsersInRoleAsync("Propietario");
                var unitOwners = usersInUnit.Where(u => owners.Any(o => o.Id == u.Id)).ToList();
                if (unitOwners.Count != 0)
                {
                    recipients.AddRange(unitOwners.Select(owner => new EmailAddress(owner.Email, owner.ToString())));
                }
                else
                {
                    return Result.Failure(NotificationErrors.NoOwnersFoundOnFineNotification);
                }
            }

            var from = new EmailAddress("intelificio@duocuc.cl", $"{assignatedFine.Fine.Community.Name} {" a través de Intelificio"}");

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "FineNotification").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnFineNotification);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnFineNotification);

            var convertedDate = TimeZoneInfo.ConvertTimeFromUtc(assignatedFine.EventDate, TZConvert.GetTimeZoneInfo(assignatedFine.Fine.Community.TimeZone));

            var templates = new List<FineNotificationTemplate>();

            foreach (var recipient in recipients)
            {
                templates.Add(new FineNotificationTemplate
                {
                    CommunityName = assignatedFine.Fine.Community.Name,
                    FineName = assignatedFine.Fine.Name,
                    FineAmount = $"{assignatedFine.Fine.Amount} {assignatedFine.Fine.Status}",
                    UnitName = assignatedFine.Unit.Number,
                    UserName = recipient.Name,
                    EventDate = convertedDate.ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    SenderAddress = $"{assignatedFine.Fine.Community.Address} {assignatedFine.Fine.Community.Municipality.Name}"
                });
            }

            if (recipients.IsNullOrEmpty()) return Result.Failure(NotificationErrors.RecipientsIsNullOrEmptyOnFineNotification);
            if (templates.IsNullOrEmpty()) return Result.Failure(NotificationErrors.TemplatesIsNullOrEmptyOnFineNotification);


            var to = new EmailAddress(assignatedFine.Unit.Users.First().Email, assignatedFine.Unit.Users.First().ToString());

            var result = await _sendMail.SendFineNotificationToMultipleRecipients(from,recipients,templateNotification.TemplateId,templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotsendOnFineNotification);
            return Result.Success();

        }

    }
}
