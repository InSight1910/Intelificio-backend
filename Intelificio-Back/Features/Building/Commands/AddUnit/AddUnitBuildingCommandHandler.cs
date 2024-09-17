using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Commands.AddUnit
{
    public class AddUnitBuildingCommandHandler : IRequestHandler<AddUnitBuildingCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<AddUnitBuildingCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddUnitBuildingCommandHandler(IntelificioDbContext context, ILogger<AddUnitBuildingCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(AddUnitBuildingCommand request, CancellationToken cancellationToken) 
        { 
            var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);
            if (building == null ) return Result.Failure(BuildingErrors.BuildingNotFoundAddUnit);

            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == request.UnitId);
            if (unit == null) return Result.Failure(BuildingErrors.UnitNotFound);

            if (building.Units.Contains(unit)) return Result.Failure(BuildingErrors.UnitAlreadyExist);

            building.Units.Add(unit);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
