using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Commands.Delete
{
    public class DeleteCommunityCommandHandler(IntelificioDbContext context, ILogger<DeleteCommunityCommandHandler> logger) : IRequestHandler<DeleteCommunityCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<DeleteCommunityCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.Include(x => x.Buildings).FirstOrDefaultAsync(x => x.ID == request.Id);
            if (community == null) return Result.Failure(CommunityErrors.CommunityNotFoundDelete);

            if (community.Buildings.Count >= 1) return Result.Failure(CommunityErrors.HasAssignedBuildingsOnDelete);
            _ = _context.Community.Remove(community);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
