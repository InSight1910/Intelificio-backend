using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryHandler : IRequestHandler<GetAllByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByCommunityQueryHandler> _logger;

        public GetAllByCommunityQueryHandler(IntelificioDbContext context, ILogger<GetAllByCommunityQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByCommunityQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);

            if (!checkCommunity) return Result.Failure(BuildingErrors.CommunityNotFoundOnQuery);
            var buildings = await _context.Buildings
                       .Where(b => b.Community.ID == request.CommunityId)
                       .Select(b => new GetAllByCommunityQueryResponse
                       {
                           Id = b.ID,
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
