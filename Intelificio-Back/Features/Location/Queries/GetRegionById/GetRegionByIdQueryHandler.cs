using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Backend.Features.Location.Queries.GetRegionById
{
    public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetRegionByIdQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Municipality.Where(x => x.ID == request.MunicipalityId).Select(x => new RegionResponse { Id = x.ID, Name = x.Name }).FirstOrDefaultAsync();

            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
