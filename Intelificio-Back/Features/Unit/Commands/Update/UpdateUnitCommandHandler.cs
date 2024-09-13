using AutoMapper;
using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.Update
{
    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<UpdateUnitCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateUnitCommandHandler(IntelificioDbContext context, ILogger<UpdateUnitCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == request.Id);

            UnitType? unitType = null;

            if (unit is null) return Result.Failure(null);

            if (request.UnitTypeID > 0)
            {
                unitType = await _context.UnitTypes.FirstOrDefaultAsync(x => x.ID == request.UnitTypeID);

                if (unitType is null) return Result.Failure(null);
            }

            unit = _mapper.Map(request, unit);

            if (unitType is not null) unit.Type = unitType;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
