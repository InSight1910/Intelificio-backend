using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.GetConcierges;

public class GetConciergesQueryHandler(IntelificioDbContext context) : IRequestHandler<GetConciergesQuery, Result>
{
    public async Task<Result> Handle(GetConciergesQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId)) return Result.Failure(null);

        var role = await context.Roles.Where(x => x.NormalizedName == "Conserje".ToUpper()).Select(x => x.Id)
            .FirstOrDefaultAsync();
        var concierges =
            await context.Users.Where(x =>
                    context.UserRoles.Any(ur => ur.RoleId == role && ur.UserId == x.Id) &&
                    context.Community.Any(x => x.ID == request.CommunityId))
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