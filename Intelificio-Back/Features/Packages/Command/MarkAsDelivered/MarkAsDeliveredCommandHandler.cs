using Backend.Common.Response;
using Backend.Features.Notification.Commands.PackageDelivered;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace Backend.Features.Packages.Command;

public class MarkAsDeliveredCommandHandler(IntelificioDbContext context, IMediator mediator)
    : IRequestHandler<MarkAsDeliveredCommand, Result>
{
    public async Task<Result> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var package = await context.Package
            .Include(x => x.Recipient)
            .Include(x => x.Concierge)
            .Include(x => x.DeliveredTo)
            .Include(x => x.CanRetire)
            .Where(x => x.ID == request.Id).FirstOrDefaultAsync();
        var community = await context.Community.Where(x => x.ID == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (community is null) return Result.Failure(PackageErrors.CommunityNotFoundOnMarkAsDelivered);

        if (!await context.Users.Where(x => x.Id == request.DeliveredToId)
                .AnyAsync(x => x.Communities.Any(c => c.ID == request.CommunityId)))
            return Result.Failure(PackageErrors.UserNoBelongToCommunity);
        if (package is null)
            return Result.Failure(PackageErrors.PackageNotFoundOnMarkAsDelivered);

        if (package.Status == PackageStatus.DELIVERED) return Result.Failure(PackageErrors.PackageAlreadyDelivered);
        if (package.CanRetire is not null || package.RecipientId == request.DeliveredToId)

            if (package.CanRetireId != request.DeliveredToId && package.RecipientId != request.DeliveredToId)
                return Result.Failure(PackageErrors.UserNotAuthorizedToRetired);
        if (package.RecipientId != request.DeliveredToId)
            return Result.Failure(PackageErrors.UserNotAuthorizedToRetired);
        package.Status = PackageStatus.DELIVERED;
        package.DeliveredToId = request.DeliveredToId;
        package.DeliveredDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TZConvert.GetTimeZoneInfo(community.TimeZone));


        await context.SaveChangesAsync();

        var packageDelivered = new PackageDeliveredCommand { DeliveredToId = request.DeliveredToId, Id = request.Id };
        _ = await mediator.Send(packageDelivered);

        return Result.Success();
    }
}