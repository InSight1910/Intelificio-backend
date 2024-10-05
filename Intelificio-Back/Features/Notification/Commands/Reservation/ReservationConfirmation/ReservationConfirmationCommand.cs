using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Reservation.ReservationConfirmation
{
    public class ReservationConfirmationCommand : IRequest<Result>
    {
        public required int ReservationID { get; set; }
    }
}
