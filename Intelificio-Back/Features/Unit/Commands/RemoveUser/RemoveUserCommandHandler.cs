using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<RemoveUserCommandHandler> _logger;

        public RemoveUserCommandHandler(IntelificioDbContext context, ILogger<RemoveUserCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.UnitId);

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundRemoveUser);

            var user = unit.Users.FirstOrDefault(x => x.Id == request.UserId);

            if (user == null) return Result.Failure(UnitErrors.UserNotFoundRemoveUser);

            if (!unit.Users.Contains(user)) return Result.Failure(UnitErrors.UserAlreadyRemoved);

            unit.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}