using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Queries.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(IntelificioDbContext context, ILogger<GetByIdQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);

            if (building == null) return Result.Failure(BuildingErrors.BuildingQueryNotFound);

            var response = _mapper.Map<GetByIdQueryResponse>(building);

            response.CommunityName = building.Community.Name;

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }
    }
}
