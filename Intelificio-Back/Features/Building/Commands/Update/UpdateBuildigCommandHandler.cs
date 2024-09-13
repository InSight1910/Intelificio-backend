using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Commands.Update
{
    public class UpdateBuildigCommandHandler : IRequestHandler<UpdateBuildingCommand,Result>
    {

        private readonly IntelificioDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateBuildigCommandHandler(IntelificioDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
           var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (building == null) return Result.Failure(BuildingErrors.BuildingUpdateNotFound);

            building = _mapper.Map(request, building);
            _ = _context.Buildings.Update(building);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
