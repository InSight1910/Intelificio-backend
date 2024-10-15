using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Features.Reservations.Query.GetReservationsById;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Xml.Linq;

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
            var reservation = await _context.Reservations.AnyAsync(x => x.ID == request.ReservationID);
            if (!reservation) return Result.Failure(NotificationErrors.ReservationNotFound);

            var reservationData = await _context.Reservations
                .Include(x => x.User)
                .Include(x => x.Spaces)
                .Where(x => x.ID == request.ReservationID)
                .Select(x =>
                new {
                    CommunityName = x.Spaces.Community.Name,
                    CommunityId = x.Spaces.CommunityId,
                    CommonSpaceName = x.Spaces.Name,
                    Capacity = x.Spaces.Capacity.ToString(),
                    Name = x.User.ToString(),
                    StartDate = TimeOnly.FromTimeSpan(x.StartTime).ToString(@"hh\:mm tt"),
                    EndDate = TimeOnly.FromTimeSpan(x.EndTime).ToString(@"hh\:mm tt"),
                    SenderAddress = string.Empty,
                    user = x.User
                }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Verificar que se haya encontrado la reservación
            if (reservationData == null)
                return Result.Failure(NotificationErrors.ReservationNotFound);

            var senderAddressData = await _context.Community
                .Include(x => x.Municipality)
                .Where(x => x.ID == reservationData.CommunityId)
                .Select(x => new {
                Address = $"{x.Address}, {x.Municipality.Name}"
                }).FirstOrDefaultAsync(cancellationToken);

            // Si no se encuentra la dirección, manejar el caso (opcional)
            if (senderAddressData == null)
                return Result.Failure("No se pudo encontrar la dirección de la comunidad.");


            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "SuccessfulReservation").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnSuccessfulReservation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnSuccessfulReservation);

            //Falta pasarle el nombre de comunidad
            var from = new EmailAddress("intelificio@duocuc.cl", $"{reservationData.CommunityName}" + " a través de Intelificio");

            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(reservationData.user.Email, reservationData.user.ToString()));

            var template = new SuccessfulReservationTemplate
            {
                CommunityName = reservationData.CommunityName,
                CommonSpaceName = reservationData.CommonSpaceName,
                Capacity = reservationData.Capacity,
                Name = reservationData.Name,
                StartDate = reservationData.StartDate,
                EndDate = reservationData.EndDate,
                SenderAddress = senderAddressData.Address,
            };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template,templateNotification.TemplateId,from,recipients);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnSuccessfulReservation);
            return Result.Success();

        }

    }
}
