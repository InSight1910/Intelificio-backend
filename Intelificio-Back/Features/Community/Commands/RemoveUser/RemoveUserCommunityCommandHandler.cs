using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.RemoveUser
{
    public class RemoveUserCommunityCommandHandler : IRequestHandler<RemoveUserCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<RemoveUserCommunityCommandHandler> _logger;

        public RemoveUserCommunityCommandHandler(IntelificioDbContext context, ILogger<RemoveUserCommunityCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(RemoveUserCommunityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing user from community start");
            _logger.LogDebug("RemoveUserCommunityCommand: Method: {Method} request: {Request}", "Community.Command.RemoveUserCommunity", request.ToString());
            var community = await _context.Community.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null) return Result.Failure(CommunityErrors.CommunityNotFoundRemoveUser);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null) return Result.Failure(CommunityErrors.UserNotFoundRemoveUser);

            if (!community.Users.Contains(user)) return Result.Failure(CommunityErrors.UserIsNotAssigned);

            _ = community.Users.Remove(user);
            _ = await _context.SaveChangesAsync();
            _logger.LogInformation("Removing user from community finish");
            return Result.Success();
        }
    }
}
