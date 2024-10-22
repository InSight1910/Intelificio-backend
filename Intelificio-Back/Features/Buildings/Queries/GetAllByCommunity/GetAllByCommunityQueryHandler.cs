using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Backend.Features.Buildings.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryHandler(IntelificioDbContext context, ILogger<GetAllByCommunityQueryHandler> logger) : IRequestHandler<GetAllByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAllByCommunityQueryHandler> _logger = logger;

        public async Task<Result> Handle(GetAllByCommunityQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId, cancellationToken);

            if (!checkCommunity) return Result.Failure(BuildingErrors.CommunityNotFoundOnQuery);
            var buildings = await _context.Buildings
                       .Where(b => b.Community.ID == request.CommunityId)
                       .Select(b => new GetAllByCommunityQueryResponse
                       {
                           Id = b.ID,
                           Name = b.Name,
                           Floors = b.Floors,
                           Units = b.Units.Count(),
                           CommunityName = b.Community.Name,
                           CommunityId = b.Community.ID
                       }).ToListAsync(cancellationToken: cancellationToken);


            return Result.WithResponse(new ResponseData()
            {
                Data = buildings
            });
        }
    }
}
