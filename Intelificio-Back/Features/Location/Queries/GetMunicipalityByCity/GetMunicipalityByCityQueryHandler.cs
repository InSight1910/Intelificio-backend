using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Location.Queries.GetMunicipalityByCity
{
    public class GetMunicipalityByCityQueryHandler : IRequestHandler<GetMunicipalityByCityQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetMunicipalityByCityQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetMunicipalityByCityQuery request, CancellationToken cancellationToken)
        {
            var municipalities = await _context.Municipality.Include(x => x.City).Where(x => x.City.ID == request.CityId).Select(x => new MunicipalityResponse { Id = x.ID, Name = x.Name }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = municipalities });
        }
    }
}
