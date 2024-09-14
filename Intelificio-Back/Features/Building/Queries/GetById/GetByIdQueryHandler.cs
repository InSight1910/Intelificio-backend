using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Queries.GetById
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
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);

            if (building == null) return Result.Failure(BuildingErrors.BuildingQueryNotFound);

            var response = _mapper.Map<GetByIDQueryResponse>(building);

            response.CommunityName = building.Community.Name;

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}
