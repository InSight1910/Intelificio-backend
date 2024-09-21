using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Queries.GetById
{
    public class GetByIDQueryHandler : IRequestHandler<GetByIDQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIDQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetByIDQueryHandler(IntelificioDbContext context, ILogger<GetByIDQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetByIDQuery request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings.Include(x => x.Community).FirstOrDefaultAsync(x => x.ID == request.BuildingId);

            if (building is null) return Result.Failure(BuildingErrors.BuildingNotFoundOnQuery);

            var response = _mapper.Map<GetByIDQueryResponse>(building);

            response.CommunityName = building.Community.Name;

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}
