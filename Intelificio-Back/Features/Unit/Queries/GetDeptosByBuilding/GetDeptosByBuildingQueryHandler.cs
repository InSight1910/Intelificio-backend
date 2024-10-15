using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetDeptosByBuilding
{
    public class GetDeptosByBuildingQueryHandler : IRequestHandler<GetDeptosByBuildingQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetDeptosByBuildingQueryHandler> _logger;

        public GetDeptosByBuildingQueryHandler(IntelificioDbContext context, ILogger<GetDeptosByBuildingQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetDeptosByBuildingQuery request, CancellationToken cancellationToken)
        {
            var units = await _context.Units
                .Include(x => x.Building)
                .Where(x => x.Building.ID == request.BuildingId && x.UnitType.Description == "Departamento")
                .Select(x => new GetDeptosByBuildingQueryResponse
                {
                    Id = x.ID,
                    Number = x.Number,
                    Building = x.Building.Name,
                    Floor = x.Floor,
                    Surface = x.Surface
                })
                .ToListAsync();

            if (units == null) return Result.Failure(UnitErrors.UnitNotFoundGetAllByBuilding);

            return Result.WithResponse(new ResponseData
            {
                Data = units
            });
        }
    }
}