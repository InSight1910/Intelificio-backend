using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Query;

public class GetCountOfReservationByCommunityAndDateQuery : IRequest<Result>
{
    public int CommunityId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}