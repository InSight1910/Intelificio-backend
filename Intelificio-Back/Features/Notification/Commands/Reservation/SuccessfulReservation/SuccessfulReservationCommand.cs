using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Reservation.SuccessfulReservation
{
    public class SuccessfulReservationCommand : IRequest<Result>
    {
        public required int ReservationID { get; set; }
    }
}
