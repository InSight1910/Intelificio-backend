using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetByID
{
    public class GetByIDQueryHandler : IRequestHandler<GetByIDQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIDQueryHandler> _logger;

        public GetByIDQueryHandler(IntelificioDbContext context, ILogger<GetByIDQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByIDQuery request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units
                .Where(x => x.ID == request.UnitId)
                .Include(x => x.UnitType)
                .Select(x => new GetByIDQueryResponse
                {
                    UnitType = x.UnitType.Description,
                    UnitTypeId = x.UnitType.ID,
                    Number = x.Number,
                    Building = x.Building.Name,
                    BuildingId = x.Building.ID,
                    Floor = x.Floor,
                    Surface = x.Surface
                }).FirstOrDefaultAsync();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundGetByID);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
