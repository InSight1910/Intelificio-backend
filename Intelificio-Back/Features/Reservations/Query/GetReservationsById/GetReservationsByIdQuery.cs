using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Query.GetReservationsById
{
    public class GetReservationsByIdQuery : IRequest<Result>
    {
        public int ReservationId { get; set; }
    }
}
