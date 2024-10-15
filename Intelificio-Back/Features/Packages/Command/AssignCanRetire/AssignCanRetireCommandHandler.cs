using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Command.AssignCanRetire;

public class AssignCanRetireCommandHandler(IntelificioDbContext context)
    : IRequestHandler<AssignCanRetireCommand, Result>
{
    public async Task<Result> Handle(AssignCanRetireCommand request, CancellationToken cancellationToken)
    {
        var package = await context.Package.Where(x => x.ID == request.PackageId).SingleOrDefaultAsync();
        if (package is null) return Result.Failure(null);

        if (!await context.Users
                .Where(x => x.Id == request.UserId)
                .AnyAsync(u => u.Communities.Any(c => c.ID == request.CommunityId))) return Result.Failure(null);

        package.CanRetireId = request.UserId;
        await context.SaveChangesAsync();

        return Result.Success();
    }
}