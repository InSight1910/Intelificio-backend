using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Location.Queries.GetCities
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetCitiesQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.City.Select(x => new CityResponse { Name = x.Name, Id = x.ID }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
