using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Commands.AddUser
{
    public class AddUserUnitCommandHandler : IRequestHandler<AddUserUnitCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<AddUserUnitCommandHandler> _logger;

        public AddUserUnitCommandHandler(IntelificioDbContext context, ILogger<AddUserUnitCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(AddUserUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _context.Units.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.UnitId);

            if (unit == null) return Result.Failure(UnitErrors.UnitNotFoundAddUser);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null) return Result.Failure(UnitErrors.UserNotFound);

            if (unit.Users.Contains(user)) return Result.Failure(UnitErrors.UserAlreadyAssigned);

            unit.Users.Add(user);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}