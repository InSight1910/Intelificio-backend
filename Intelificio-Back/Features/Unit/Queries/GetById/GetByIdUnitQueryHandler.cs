using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetById
{
    public class GetByIdUnitQueryHandler : IRequestHandler<GetByIdUnitQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIdUnitQueryHandler> _logger;

        public GetByIdUnitQueryHandler(IntelificioDbContext context, ILogger<GetByIdUnitQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByIdUnitQuery request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units
                .Where(x => x.ID == request.UnitId)
                .Include(x => x.UnitType)
                .Select(x => new GetByIdUnitQueryResponse
                {
                    UnitType = x.UnitType.Description,
                    UnitTypeId = x.UnitType.ID,
                    Number = x.Number,
                    Building = x.Building.Name,
                    BuildingId = x.Building.ID,
                    Floor = x.Floor,
                    Surface = x.Surface
                }).FirstOrDefaultAsync();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundGetById);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
