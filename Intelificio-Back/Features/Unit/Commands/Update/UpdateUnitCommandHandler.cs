using AutoMapper;
using Backend.Common.Response;
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
            if (unit is null) return Result.Failure(null);

            // Update unidad

            UnitType? unitType = null;

            if (request.UnitTypeId > 0)
            {
                unitType = await _context.UnitTypes.FirstOrDefaultAsync(x => x.ID == request.UnitTypeId);
                if (unitType is null) return Result.Failure(null);
            }
            unit = _mapper.Map(request, unit);
            if (unitType is not null) unit.UnitType = unitType;

            // Update edificio

            Building? building = null;

            if (request.BuildingId > 0)
            {
                building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);
                if (building is null) return Result.Failure(null);
            }

            unit = _mapper.Map(request, unit);
            if (building is not null) unit.Building = building;

            // Update numero
            if (!string.IsNullOrEmpty(request.Number))
            {
                unit.Number = request.Number;
            }

            //Update piso
            if (request.Floor.HasValue)
            {
                unit.Floor = request.Floor.Value;
            }

            // Update superficie
            if (request.Surface > 0) 
            {
                unit.Surface = request.Surface;
            }

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
