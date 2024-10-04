using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.Reservation.ReservationCancellation
{
    public class ReservationCancellationCommand : IRequest<Result>
    {
        public required int ReservationID { get; set; }
    }
}
