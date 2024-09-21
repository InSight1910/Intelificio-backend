using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Location.Queries.GetCitiesByRegion
{
    public class GetCitiesByRegionQueryHandler : IRequestHandler<GetCitiesByRegionQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetCitiesByRegionQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetCitiesByRegionQuery request, CancellationToken cancellationToken)
        {
            var cities = await _context.City.Include(x => x.Region).Where(x => x.Region.ID == request.RegionId).Select(x => new CityResponse { Id = x.ID, Name = x.Name }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = cities });
        }
    }
}
