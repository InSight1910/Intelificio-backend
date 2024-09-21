using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetUsersByCommunity
{
    public class GetUsersByCommunityQueryHandler : IRequestHandler<GetUsersByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetUsersByCommunityQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetUsersByCommunityQuery request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.Include(c => c.Users).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null)
            {
                return Result.Failure(null);
            }

            var users = community.Users.Select(u => new GetUsersByCommunityQueryResponse
            {
                Id = u.Id,
                Name = string.Format("{0} {1}", u.FirstName, u.LastName),
                Email = u.Email!,
                PhoneNumber = u.PhoneNumber,
                UnitCount = _context.Units.Count(x => x.ID == u.Id),
                Role = u.Role.Name!
            }).ToList();

            return Result.WithResponse(new ResponseData
            {
                Data = users
            });
        }
    }
}
