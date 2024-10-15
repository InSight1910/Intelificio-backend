using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Query.GetReservationsByUser;

public class GetReservationsByUserQuery : IRequest<Result>
{
    public int UserId { get; set; }
}