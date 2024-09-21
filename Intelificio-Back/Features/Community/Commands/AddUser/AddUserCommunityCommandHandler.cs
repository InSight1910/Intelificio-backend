using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Assign
{
    public class AddUserCommunityCommandHandler : IRequestHandler<AddUserCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<AddUserCommunityCommandHandler> _logger;

        public AddUserCommunityCommandHandler(IntelificioDbContext context, ILogger<AddUserCommunityCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(AddUserCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null) return Result.Failure(CommunityErrors.CommunityNotFoundAddUser);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null) return Result.Failure(CommunityErrors.UserNotFoundAddUser);

            if (community.Users.Any(x => x.Id == user.Id)) return Result.Failure(CommunityErrors.UserAlreadyInCommunity);

            community.Users.Add(user);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
