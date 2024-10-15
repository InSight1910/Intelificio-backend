using Backend.Common.Response;
using Backend.Features.Reservations.Query.GetCountOfReservationByCommunityAndDate;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Reservations.Query.GetReservationsByCommunityAndMonth;

public class GetCountOfReservationByCommunityAndDateQueryHandler(IntelificioDbContext context)
    : IRequestHandler<GetCountOfReservationByCommunityAndDateQuery, Result>
{
    public async Task<Result> Handle(GetCountOfReservationByCommunityAndDateQuery request,
        CancellationToken cancellationToken)
    {
        var checkCommunity = await context.Community.AnyAsync(x => x.ID == request.CommunityId);
        if (!checkCommunity) return Result.Failure(null);


        var reservation = await context.Reservations
            .Where(x => x.Date.Month == request.Month && x.Date.Year == request.Year)
            .GroupBy(e => e.Date.Day)
            .Select(x => new GetCountOfReservationByCommunityAndDateQueryResponse
            {
                day = (int)x.Key,
                countReservations = x.GroupBy(statusGroup => statusGroup.Status).Select(y => new CountReservations()
                {
                    Status = (int)y.Key,
                    Count = y.Count()
                }).ToList()
            })
            .ToListAsync(cancellationToken);
        return Result.WithResponse(new ResponseData { Data = reservation });
    }
}