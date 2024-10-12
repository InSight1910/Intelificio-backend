using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Reservation.SuccessfulReservation;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.Reservation.ReservationCancellation
{
    public class ReservationCancellationHandler : IRequestHandler<ReservationCancellationCommand,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<ReservationCancellationHandler> _logger;

        public ReservationCancellationHandler(IntelificioDbContext context, ILogger<ReservationCancellationHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(ReservationCancellationCommand request, CancellationToken cancellationToken)
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


            var recipients = new List<EmailAddress>();
            recipients.Add(new EmailAddress(reservationData.user.Email, reservationData.user.ToString() ));

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "ReservationCancellation").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnReservationCancellation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnReservationCancellation);

            //Falta pasarle el nombre de comunidad
            var from = new EmailAddress("intelificio@duocuc.cl", $"{reservationData.CommunityName}" + " a través de Intelificio");

            var template = new ReservationCancellationTemplate
            {
                CommunityName = reservationData.CommunityName,
                CommonSpaceName = reservationData.CommonSpaceName,
                Name = reservationData.user.ToString(),
                StartDate = reservationData.StartDate,
                EndDate = reservationData.EndDate,
                SenderAddress = senderAddressData.Address
            };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template, templateNotification.TemplateId, from, recipients);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnReservationCancellation);
            return Result.Success();

        }

    }
}
