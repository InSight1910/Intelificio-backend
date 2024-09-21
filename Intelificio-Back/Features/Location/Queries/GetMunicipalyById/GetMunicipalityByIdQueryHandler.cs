using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Backend.Features.Location.Queries.GetMunicipalyById
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetMunicipalityByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetCityByIdQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetMunicipalityByIdQuery request, CancellationToken cancellationToken)
        {
            var municipality = await _context.Municipality.FirstOrDefaultAsync(x => x.ID == request.MunicipalityId);
            var result = _mapper.Map<MunicipalityResponse>(municipality);
            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
