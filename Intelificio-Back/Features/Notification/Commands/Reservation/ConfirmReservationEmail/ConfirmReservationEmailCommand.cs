using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Reservation.ReservationConfirmation
{
    public class ConfirmReservationEmailCommand : IRequest<Result>
    {
        public required int ReservationID { get; set; }
    }
}
