using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
           
            if (unit is null) return Result.Failure(UnitErrors.UnitNotFoundUpdate);

            unit = _mapper.Map(request, unit);

            // Update unidad

            UnitType? unitType = null;

            if (request.UnitTypeId > 0)
            {
                unitType = await _context.UnitTypes.FirstOrDefaultAsync(x => x.ID == request.UnitTypeId);
                if (unitType is null) return Result.Failure(UnitErrors.UnitTypeNotFoundUpdate);
            }

            if (unitType is not null) unit.UnitType = unitType;

            // Update edificio

            Building? building = null;

            if (request.BuildingId > 0)
            {
                building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);
                if (building is null) return Result.Failure(UnitErrors.BuildingNotFoundUpdate);
            }

            if (building is not null) unit.Building = building;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
