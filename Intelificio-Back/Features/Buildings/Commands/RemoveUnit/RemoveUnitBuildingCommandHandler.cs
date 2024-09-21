using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Commands.RemoveUnit
{
    public class RemoveUnitBuildingCommandHandler : IRequestHandler<RemoveUnitBuildingCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<RemoveUnitBuildingCommandHandler> _logger;
        private readonly IMapper _mapper;

        public RemoveUnitBuildingCommandHandler(IntelificioDbContext context, ILogger<RemoveUnitBuildingCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(RemoveUnitBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);
            if (building == null) return Result.Failure(BuildingErrors.BuildingNotFoundOnRemoveUnit);

            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == request.UnitId);
            if (unit == null) return Result.Failure(BuildingErrors.UnitNotFoundOnRemoveUnit);

            if (!building.Units.Contains(unit)) return Result.Failure(BuildingErrors.UnitDoesNotExistInBuildingOnRemoveUnit);

            _ = building.Units.Remove(unit);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
