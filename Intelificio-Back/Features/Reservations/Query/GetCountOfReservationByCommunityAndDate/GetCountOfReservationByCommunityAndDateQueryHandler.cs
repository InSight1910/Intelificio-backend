using Backend.Common.Response;
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
        var checkCommunity = await context.Community.AnyAsync(x => x.ID == request.communityId);
        if (!checkCommunity) return Result.Failure(null);

        var reservation = await context.Reservations
            .Where(x => x.Date.Date == request.date.Date)
            .Select(
                x => x)
            .ToListAsync(cancellationToken);
        return Result.Success();
    }
}