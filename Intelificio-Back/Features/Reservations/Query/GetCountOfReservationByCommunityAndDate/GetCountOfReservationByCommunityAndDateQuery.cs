using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Query;

public class GetCountOfReservationByCommunityAndDateQuery : IRequest<Result>
{
    public int communityId { get; set; }
    public DateTime date { get; set; }
}