using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Command.Create;

public class CreatePackageCommandHandler(IntelificioDbContext context, IMapper mapper)
    : IRequestHandler<CreatePackageCommand, Result>
{
    public async Task<Result> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId))
            return Result.Failure(PackageErrors.CommunityNotFoundOnCreate);

        var conciergeRoleId = await context.Roles.Where(x => x.Name == "Conserje").Select(x => x.Id).FirstAsync();
        if (await context.Users
                .Include(x => x.Communities)
                .Where(x =>
                    x.Id == request.ConciergeId &&
                    context.UserRoles.Any(ur => ur.RoleId == conciergeRoleId && ur.UserId == x.Id) &&
                    x.Communities.Any(c => c.ID == request.CommunityId)).AnyAsync())
            return Result.Failure(PackageErrors.ConciergeNotFoundOnCreate);

        if (!await context.Users.AnyAsync(x =>
                x.Id == request.RecipientId && x.Communities.Any(x => x.ID == request.CommunityId)))
            return Result.Failure(PackageErrors.RecipientNotFoundOnCreate);

        if (await context.Package.AnyAsync(x =>
                x.TrackingNumber == request.TrackingNumber && x.CommunityId == request.CommunityId))
            return Result.Failure(PackageErrors.PackageAlreadyExistOnCreate);

        var package = mapper.Map<Package>(request);


        var result = await context.Package.AddAsync(package);
        await context.SaveChangesAsync(cancellationToken);

        var response = new CreatePackageCommandResponse
        {
            Id = result.Entity.ID,
            Status = (int)result.Entity.Status,
            ReceptionDate = result.Entity.ReceptionDate,
            ConciergeName = await context.Users.Where(x => x.Id == result.Entity.ConciergeId).Select(x => x.ToString())
                .FirstAsync(),
            RecipientName = await context.Users.Where(x => x.Id == result.Entity.RecipientId).Select(x => x.ToString())
                .FirstAsync(),
            TrackingNumber = result.Entity.TrackingNumber
        };

        return Result.WithResponse(new ResponseData { Data = response });
    }
}