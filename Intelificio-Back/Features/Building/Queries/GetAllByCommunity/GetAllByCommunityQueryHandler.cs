using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Queries.GetAllByCommunity
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
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);

            if (!checkCommunity) return Result.Failure(BuildingErrors.CommunityNotFoundOnQuery);
            var buildings = await _context.Buildings
                       .Where(b => b.Community.ID == request.CommunityId)
                       .Select(b => new GetAllByCommunityQueryResponse
                       {
                           Name = b.Name,
                           Floors = b.Floors
                       }).ToListAsync(cancellationToken: cancellationToken);


            return Result.WithResponse(new ResponseData()
            {
                Data = buildings
            });
        }
    }
}
