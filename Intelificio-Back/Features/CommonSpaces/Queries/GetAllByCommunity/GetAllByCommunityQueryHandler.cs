using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.CommonSpaces.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryHandler : IRequestHandler<GetAllByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetAllByCommunityQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllByCommunityQuery request, CancellationToken cancellationToken)
        {
            var existCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);
            if (!existCommunity) return Result.Failure("Community not found");

            var commonSpaces = await _context.CommonSpaces
                .Where(x => x.CommunityId == request.CommunityId)
                .Select(x => new GetAllByCommunityQueryResponse
                {
                    ID = x.ID,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Location = x.Location,
                    IsInMaintenance = x.IsInMaintenance
                })
                .ToListAsync(cancellationToken);

            return Result.WithResponse(new ResponseData { Data = commonSpaces });
        }
    }
}
