using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Queries.GetByUser;

public class GetByUserQueryHandler(IntelificioDbContext context) : IRequestHandler<GetByUserQuery, Result>
{
    public async Task<Result> Handle(GetByUserQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId)) return Result.Failure(null);
        if (!await context.Users.AnyAsync(x =>
                x.Id == request.UserId && context.Community.Any(c => c.Users.Any(u => u.Id == request.UserId))))
            return Result.Failure(null);

        var packages = await context.Package
            .Include(x => x.Concierge)
            .Include(x => x.CanRetire)
            .Where(x => x.RecipientId == request.UserId)
            .OrderBy(x => x.Status)
            .Select(x => new GetByUserQueryResponse
            {
                Id = x.ID,
                ReceptionDate = x.ReceptionDate,
                TrackingNumber = x.TrackingNumber,
                ConciergeName = x.Concierge.ToString(),
                Status = x.Status,
                AssignedTo = x.CanRetire == null ? null : x.CanRetire.ToString()
            })
            .ToListAsync();

        return Result.WithResponse(new ResponseData
        {
            Data = packages
        });
    }
}