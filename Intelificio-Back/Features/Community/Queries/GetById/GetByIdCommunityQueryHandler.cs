using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetById
{
    public class GetByIdCommunityQueryHandler : IRequestHandler<GetByIdCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIdCommunityQueryHandler> _logger;

        public GetByIdCommunityQueryHandler(IntelificioDbContext context, ILogger<GetByIdCommunityQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByIdCommunityQuery request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.Where(x => x.ID == request.Id).Select(x => new GetByIdCommunityResponse
            {
                Id = x.ID,
                Address = x.Address,
                MunicipalityId = x.Municipality.ID,
                CityId = x.Municipality.City.ID,
                RegionId = x.Municipality.City.Region.ID,
                Name = x.Name
            }).FirstOrDefaultAsync();

            if (community is null) return Result.Failure(CommunityErrors.CommunityNotFoundGetByID);

            return Result.WithResponse(new ResponseData
            {
                Data = community
            });
        }
    }
}
