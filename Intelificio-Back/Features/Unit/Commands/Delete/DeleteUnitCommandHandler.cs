using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.Delete
{
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<DeleteUnitCommandHandler> _logger;

        public DeleteUnitCommandHandler(IntelificioDbContext context, ILogger<DeleteUnitCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundDelete);

            _ = _context.Units.Remove(unit);
            _ = await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
