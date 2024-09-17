using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAll
{
    public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, Result>
    {
        private readonly ILogger<GetAllCommunitiesQueryHandler> _logger;
        private readonly IntelificioDbContext _context;

        public GetAllCommunitiesQueryHandler(ILogger<GetAllCommunitiesQueryHandler> logger, IntelificioDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            var communities = await _context.Community
                .Include(x => x.Municipality.City.Region)
                .Select(x => new GetAllCommunitiesResponse
                {
                    Address = x.Address,
                    CreationDate = x.FoundationDate,
                    Name = x.Name,
                    Municipality = x.Municipality.Name,
                    City = x.Municipality.City.Name,
                    Region = x.Municipality.City.Region.Name
                })
                .ToListAsync();
            return Result.WithResponse(new ResponseData
            {
                Data = communities
            });
        }
    }
}
