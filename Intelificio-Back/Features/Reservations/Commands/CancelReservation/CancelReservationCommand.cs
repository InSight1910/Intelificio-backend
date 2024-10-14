using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommand : IRequest<Result>
    {
        public int ReservationId { get; set; }
    }
}
