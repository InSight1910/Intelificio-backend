using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Backend.Features.Location.Queries.GetCityById
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.City.Where(x => x.ID == request.CityId).Select(x => new CityResponse
            {
                Id = x.ID,
                Name = x.Name
            }).FirstOrDefaultAsync();

            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
