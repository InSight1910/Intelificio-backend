﻿using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Buildings.Commands.Delete
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

            _ = _context.Buildings.Remove(building);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }

    }
}
