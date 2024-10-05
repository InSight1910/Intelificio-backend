using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Reservation.SuccessfulReservation;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Backend.Features.Notification.Commands.Reservation.ReservationConfirmation
{
    public class ReservationConfirmationHandler : IRequestHandler<ReservationConfirmationCommand,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<ReservationConfirmationHandler> _logger;

        public ReservationConfirmationHandler(IntelificioDbContext context, ILogger<ReservationConfirmationHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(ReservationConfirmationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .Where(r => r.ID == request.ReservationID)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (reservation == null) return Result.Failure(NotificationErrors.ReservationNotFound);

            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(reservation.User.Email, $"{reservation.User.FirstName} {reservation.User.LastName}"));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ReservationConfirmation").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnReservationConfirmation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnReservationConfirmation);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{reservation.Spaces.Community.Name}" + " a través de Intelificio");

            var template = new ReservationConfirmationTemplate
            {
                CommunityName = reservation.Spaces.Community.Name,
                CommonSpaceName = reservation.Spaces.Name,
                UserName = $"{reservation.User.FirstName} {reservation.User.LastName}",
                ConfirmLink = $"http://localhost:4200/confirmReservation?Id={reservation.ID}",

            };

            if (template == null ||
                string.IsNullOrEmpty(template.CommunityName) ||
                string.IsNullOrEmpty(template.CommonSpaceName) ||
                string.IsNullOrEmpty(template.UserName) ||
                string.IsNullOrEmpty(template.ConfirmLink))
            {
                return Result.Failure(NotificationErrors.TemplateNotCreatedOnReservationConfirmation);
            }

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            templateNotification.TemplateId,
                                                            from,
                                                            recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnReservationConfirmation);
            return Result.Success();

        }
    }
}
