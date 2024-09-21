using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetByUser
{
    public class GetByUserQueryHandler : IRequestHandler<GetByUserQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByUserQueryHandler> _logger;

        public GetByUserQueryHandler(IntelificioDbContext context, ILogger<GetByUserQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByUserQuery request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units
                .Where(x => x.Users.Any(user => user.Id == request.UserId))
                .Select(x => new GetByUserQueryResponse
                {
                    Number = x.Number,
                    UnitType = x.UnitType.Description,
                    Surface = x.Surface,
                    Floor = x.Floor,
                    Building = x.Building.Name
                })
                .ToListAsync();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundGetByUser);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
