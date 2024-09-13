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
        
        public DeleteBuildingCommandHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
           var building = await _context.Buildings.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (building == null) return Result.Failure(BuildingErrors.BuildingNotFound);
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
