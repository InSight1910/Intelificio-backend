using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.Queries.GetConcierges;

public class GetConciergesQueryHandler(IntelificioDbContext context) : IRequestHandler<GetConciergesQuery, Result>
{
    public async Task<Result> Handle(GetConciergesQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId)) return Result.Failure(null);

        var role = await context.Roles.Where(x => x.NormalizedName == "Conserje".ToUpper()).Select(x => x.Id)
            .FirstAsync();
        var concierges =
            await context.Users
                .Include(x => x.Communities)
                .Where(x =>
                    context.UserRoles.Any(ur => ur.RoleId == role && ur.UserId == x.Id) &&
                    x.Communities.Any(c => c.ID == request.CommunityId))
                .Select(x => new GetConciergesQueryResponse
                {
                    Id = x.Id,
                    Name = x.ToString()
                })
                .ToListAsync();
        return Result.WithResponse(new ResponseData
        {
            Data = concierges
        });
    }
}