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
            var checkCommunity =  _context.Community.Any(x => x.ID == request.CommunityId);

            if (!checkCommunity) return Result.Failure(BuildingErrors.CommunityQueryNotFound);
            var buildings = await _context.Community
                                  .Where(x => x.ID == request.CommunityId)
                                  .Include(x => x.Buildings)
                                  .Select(x => new GetAllByCommunityQueryResponse
                                  {
                                      Floors = x.Buildings.Select(b => b.Floors).FirstOrDefault(),
                                      Name = x.Buildings.Select(b => b.Name).FirstOrDefault()
                                  }).ToListAsync(cancellationToken: cancellationToken);
                                  

            return Result.WithResponse(new ResponseData()
            {
                Data = buildings
            });
        }
    }
}
