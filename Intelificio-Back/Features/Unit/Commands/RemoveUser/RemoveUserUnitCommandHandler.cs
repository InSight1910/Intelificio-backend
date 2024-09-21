using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.RemoveUser
{
    public class RemoveUserUnitCommandHandler : IRequestHandler<RemoveUserUnitCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<RemoveUserUnitCommandHandler> _logger;

        public RemoveUserUnitCommandHandler(IntelificioDbContext context, ILogger<RemoveUserUnitCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(RemoveUserUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.UnitId);

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundRemoveUser);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null) return Result.Failure(UnitErrors.UserNotFoundRemoveUser);

            if (!unit.Users.Contains(user)) return Result.Failure(UnitErrors.UserIsNotAssigned);

            unit.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}