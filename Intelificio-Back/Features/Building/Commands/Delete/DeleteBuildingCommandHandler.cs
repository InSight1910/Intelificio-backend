using Backend.Common.Response;
using Backend.Features.Building.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Building.Commands.Delete
{
    public class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<DeleteBuildingCommandHandler> _logger;
        
        public DeleteBuildingCommandHandler(IntelificioDbContext context, ILogger<DeleteBuildingCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
           var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.Id);
       
           if (building == null) return Result.Failure(BuildingErrors.BuildingNotFoundOnDelete);

           if (building.Units.Count >= 1) return Result.Failure(BuildingErrors.HasAssignedUnitsOnDelete); 

            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
