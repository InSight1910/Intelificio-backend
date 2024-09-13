using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetByUserQueryHandler(IntelificioDbContext context, ILogger<GetByUserQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetByUserQuery request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units
                .Where(x => x.Users.Any(user => user.Id == request.UserId))
                .Select(x => new GetByUserQueryResponse
                {
                    Number = x.Number,
                    UnitType = x.UnitType,
                    Surface = x.Surface,
                    Floor = x.Floor,
                    Building = x.Building
                })
                .ToListAsync();

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFound);

            return Result.WithResponse(new ResponseData()
            {
                Data = unit
            });
        }
    }
}
