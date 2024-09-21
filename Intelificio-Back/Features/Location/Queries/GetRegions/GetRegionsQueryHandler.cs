using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Location.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Location.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly IMapper _mapper;

        public GetRegionsQueryHandler(IntelificioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Regions.Select(x => new RegionResponse { Name = x.Name, Id = x.ID }).ToListAsync();
            return Result.WithResponse(new ResponseData { Data = result });
        }
    }
}
