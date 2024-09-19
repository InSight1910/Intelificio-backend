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
                        UnitType = x.UnitType,
                        Number = x.Number,
                        Building = x.Building,
                        Floor = x.Floor,
                        Surface = x.Surface
                    })
                    .ToListAsync();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundGetByID);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
