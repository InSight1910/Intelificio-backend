using Backend.Common.Response;
using Backend.Features.Reservations.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Query.GetReservationsByCommunityAndMonth;

public class GetReservationsByCommunityAndMonthQueryHandler(IntelificioDbContext context)
    : IRequestHandler<GetReservationsByCommunityAndMonthQuery, Result>
{
    public async Task<Result> Handle(GetReservationsByCommunityAndMonthQuery request,
        CancellationToken cancellationToken)
    {
        var checkCommunity = await context.Community.AnyAsync(x => x.ID == request.CommunityId, cancellationToken);
        if (!checkCommunity) return Result.Failure(ReservationErrors.CommunityNotFoundOnGetByCommunityAndMonth);

        TimeOnly time;
        var result = await context.Reservations
            .Include(x => x.User)
            .Include(x => x.Spaces)
            .Where(x => x.Date.Date == request.Date)
            .Select(x => new GetReservationsByCommunityAndMonthQueryResponse
            {
                Id = x.ID,
                UserName = x.User.ToString(),
                SpaceName = x.Spaces.Name,
                Status = (int)x.Status,
                Date = x.Date.ToString(@"yyyy-MM-dd"),
                StartTime = x.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = x.EndTime.ToString(@"hh\:mm\:ss")
            })
            .ToListAsync(cancellationToken);
        return Result.WithResponse(new ResponseData { Data = result });
    }
}