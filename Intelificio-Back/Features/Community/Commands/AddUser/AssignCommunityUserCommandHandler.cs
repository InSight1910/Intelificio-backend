using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Assign
{
    public class AssignCommunityUserCommandHandler : IRequestHandler<AssignCommunityUserCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<AssignCommunityUserCommand> _logger;

        public AssignCommunityUserCommandHandler(IntelificioDbContext context, ILogger<AssignCommunityUserCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(AssignCommunityUserCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null) return Result.Failure(null);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null) return Result.Failure(null);

            community.Users.Add(user);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
