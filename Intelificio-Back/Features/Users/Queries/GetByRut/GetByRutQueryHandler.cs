using Backend.Common.Response;
using Backend.Features.Users.Queries.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.Queries.GetByRut;

public class GetByRutQueryHandler(IntelificioDbContext context) : IRequestHandler<GetByRutQuery, Result>
{
    public async Task<Result> Handle(GetByRutQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(x => x.Rut == request.Rut && x.Communities.Any(c => x.Id == request.CommunityId) || request.CommunityId == null)
            .Select(x => new
            {
                Id = x.Id,
                Name = x.ToString()
            }).FirstOrDefaultAsync();

        if (user is null) return Result.Failure(UsersError.UserNotFoundOnQuery);

        return Result.WithResponse(new ResponseData()
        {
            Data = user
        });
    }
}