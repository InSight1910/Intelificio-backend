using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.Create
{
    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateUnitCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateUnitCommandHandler(IntelificioDbContext context, ILogger<CreateUnitCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var checkBuilding = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.BuildingId);

            if (checkBuilding == null) return Result.Failure(UnitErrors.BuildingNotFound);

            var checkUnitType = await _context.UnitTypes.FirstOrDefaultAsync(x => x.ID == request.UnitTypeId);

            if (checkUnitType == null) return Result.Failure(UnitErrors.UnitTypeNotFound);


            var newUnit = _mapper.Map<Models.Unit>(request);

            newUnit.Building = checkBuilding;

            newUnit.Type = checkUnitType;

            await _context.Units.AddAsync(newUnit);

            return Result.Success();

        }
    }
}
