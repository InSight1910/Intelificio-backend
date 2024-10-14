using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Package;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Command.Create;

public class CreatePackageCommandHandler(IntelificioDbContext context, IMapper mapper, IMediator mediator)
    : IRequestHandler<CreatePackageCommand, Result>
{
    public async Task<Result> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId))
            return Result.Failure(PackageErrors.CommunityNotFoundOnCreate);

        var conciergeRoleId = await context.Roles.Where(x => x.Name == "Conserje").Select(x => x.Id).FirstAsync();
        if (!await context.Users.AnyAsync(x => x.Id == request.ConciergeId) &&
            await context.UserRoles.AnyAsync(x => x.UserId == request.ConciergeId && x.RoleId == conciergeRoleId))
            return Result.Failure(PackageErrors.ConciergeNotFoundOnCreate);

        if (!await context.Users.AnyAsync(x =>
                x.Id == request.RecipientId && x.Communities.Any(x => x.ID == request.CommunityId)))
            return Result.Failure(PackageErrors.RecipientNotFoundOnCreate);

        if (await context.Package.AnyAsync(x =>
                x.TrackingNumber == request.TrackingNumber && x.CommunityId == request.CommunityId))
            return Result.Failure(PackageErrors.PackageAlreadyExistOnCreate);

        var package = mapper.Map<Package>(request);

        var result = await context.Package.AddAsync(package);
        var packageCreated = await context.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<CreatePackageCommandResponse>(result.Entity);

        var packageCommand = new PackageCommand { PackageID = response.Id };
        _ = await mediator.Send(packageCommand);

        return Result.WithResponse(new ResponseData { Data = response });
    }
}