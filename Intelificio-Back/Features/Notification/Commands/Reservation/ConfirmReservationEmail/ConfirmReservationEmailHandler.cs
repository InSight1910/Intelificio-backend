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
    public class ConfirmReservationEmailHandler : IRequestHandler<ConfirmReservationEmailCommand,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<ConfirmReservationEmailHandler> _logger;

        public ConfirmReservationEmailHandler(IntelificioDbContext context, ILogger<ConfirmReservationEmailHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(ConfirmReservationEmailCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations.AnyAsync(x => x.ID == request.ReservationID);
            if (!reservation) return Result.Failure(NotificationErrors.ReservationNotFound);

            var result = await _context.Reservations
                .Include(x => x.User)
                .Include(x => x.Spaces)
                .Where(x => x.ID == request.ReservationID)
                .Select(x =>
                new {
                    CommunityName = x.Spaces.Community.Name,
                    CommonSpaceName = x.Spaces.Name,
                    user = x.User,
                    id = x.ID,
                    ConfirmationToken = x.ConfirmationToken
                }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (result is null) return Result.Failure(NotificationErrors.CommunityNotfoundOnConfirmReservationEmail);

            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(result.user.Email, result.user.ToString()));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ReservationConfirmation").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnReservationConfirmation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnReservationConfirmation);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{result.CommunityName}" + " a través de Intelificio");

            var template = new ConfirmReservationEmailTemplate
            {
                CommunityName = result.CommunityName,
                CommonSpaceName = result.CommonSpaceName,
                UserName = result.user.ToString(),
                ConfirmLink = $"http://localhost:4200/ConfirmarReserva?reservationId={result.id}&token={WebUtility.UrlEncode(result.ConfirmationToken)}",

            };

            if (template == null ||
                string.IsNullOrEmpty(template.CommunityName) ||
                string.IsNullOrEmpty(template.CommonSpaceName) ||
                string.IsNullOrEmpty(template.UserName) ||
                string.IsNullOrEmpty(template.ConfirmLink))
            {
                return Result.Failure(NotificationErrors.TemplateNotCreatedOnReservationConfirmation);
            }

            var resultado = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            templateNotification.TemplateId,
                                                            from,
                                                            recipients
                                                        );

            if (!resultado.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnReservationConfirmation);
            return Result.Success();

        }
    }
}
