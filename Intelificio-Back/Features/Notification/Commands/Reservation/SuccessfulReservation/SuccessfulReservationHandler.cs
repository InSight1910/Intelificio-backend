using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.Reservation.SuccessfulReservation
{
    public class SuccessfulReservationHandler : IRequestHandler<SuccessfulReservationCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SuccessfulReservationHandler> _logger;

        public SuccessfulReservationHandler(IntelificioDbContext context, ILogger<SuccessfulReservationHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(SuccessfulReservationCommand request, CancellationToken cancellationToken)
        {
            //Completar la Query para obtener la reserva y confirmar su estado, datos del espacio común, comunidad y usuario. 
            var reservation = await _context.Reservations
                    .Where(r => r.ID == request.ReservationID).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(
            //package.Owner.Email ?? "intelificio@duocuc.cl",
            //$"{package.Owner.FirstName} {package.Owner.LastName}"
            ));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.ID == 4).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnSuccessfulReservation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnSuccessfulReservation);

            //Falta pasarle el nombre de comunidad
            var from = new EmailAddress("intelificio@duocuc.cl", $"{"NombreCOMUNIDAD"}" + " a través de Intelificio");

            var template = new SuccessfulReservationTemplate
            {
                CommunityName = "",
                CommonSpaceName = "",
                Capacity = "",
                Name = "",
                StartDate = "",
                EndDate = "",
                SenderAddress = "",
            };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template, templateNotification.TemplateId, from, recipients);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnSuccessfulReservation);
            return Result.Success();

        }

    }
}
