using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Reservations.Query.GetReservationsByCommunityAndMonth;

public class GetReservationsByCommunityAndMonthQuery : IRequest<Result>
{
    public required DateTime Date { get; set; }
    public int CommunityId { get; set; }
}