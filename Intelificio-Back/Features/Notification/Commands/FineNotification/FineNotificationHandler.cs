using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.FineNotification
{
    public class FineNotificationHandler(IntelificioDbContext context, ILogger<FineNotificationHandler> logger, SendMail sendMail) : IRequestHandler<FineNotificationCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<FineNotificationHandler> _logger = logger;
        private readonly SendMail _sendMail = sendMail;

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

            var from = new EmailAddress("intelificio@duocuc.cl", $"{assignatedFine.Fine.Community.Name} {" a través de Intelificio"}");

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "FineNotification").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnFineNotification);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnFineNotification);

            var template = new FineNotificationTemplate
            {
                CommunityName = assignatedFine.Fine.Community.Name,
                FineName = assignatedFine.Fine.Name,
                FineAmount = $"{assignatedFine.Fine.Amount} {assignatedFine.Fine.Status}",
                UnitName = assignatedFine.Unit.Number,
                UserName = assignatedFine.Unit.Users.First().ToString(),
                EventDate = assignatedFine.EventDate.ToString(),
                SenderAddress = $"{assignatedFine.Fine.Community.Address} {assignatedFine.Fine.Community.Municipality.Name}"
            };

            var to = new EmailAddress(assignatedFine.Unit.Users.First().Email, assignatedFine.Unit.Users.First().ToString());

            var result = await _sendMail.SendFineNotificationToMultipleRecipients(from, to, templateNotification.TemplateId, template);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotsendOnFineNotification);
            return Result.Success();

        }

    }
}
