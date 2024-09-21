using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Location.Queries.GetMunicipalities
{
    public class GetMunicipalitiesQueryHandler : IRequestHandler<GetMunicipalitiesQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetMunicipalitiesQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetMunicipalitiesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Municipality.Select(x => new CityResponse { Name = x.Name, Id = x.ID }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
