using Backend.Common.Response;
using Backend.Features.Community.Commands.AddUser;
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
            if (request.User != null)
            {
                var result = await DoAddUsers(request.User);
                return result;
            }
            else if (request.Users != null)
            {
                var results = new List<Result>();
                foreach (var user in request.Users)
                {
                    results.Add(await DoAddUsers(user));
                }

                if (results.Any(r => r.IsFailure)) return Result.WithErrors(CommunityErrors.AddUserMassive(results.Select(r => r.Error).ToList()));
                return Result.Success();
            }
            return Result.Failure(null);
        }

        private async Task<Result> DoAddUsers(AddUserObject request)
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
