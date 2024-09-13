using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.RemoveUser
{
    public class RemoveUserCommunityCommandHandler : IRequestHandler<RemoveUserCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<RemoveUserCommunityCommand> _logger;

        public RemoveUserCommunityCommandHandler(IntelificioDbContext context, ILogger<RemoveUserCommunityCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(RemoveUserCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null) return Result.Failure(null);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null) return Result.Failure(null);

            if (!community.Users.Contains(user)) return Result.Failure(null);

            _ = community.Users.Remove(user);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
