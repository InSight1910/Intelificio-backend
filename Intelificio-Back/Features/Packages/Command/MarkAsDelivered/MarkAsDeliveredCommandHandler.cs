using Backend.Common.Response;
using Backend.Features.Notification.Commands.PackageDelivered;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Command;

public class MarkAsDeliveredCommandHandler(IntelificioDbContext context, IMediator mediator)
    : IRequestHandler<MarkAsDeliveredCommand, Result>
{
    public async Task<Result> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var package = await context.Package.Where(x => x.ID == request.Id).FirstOrDefaultAsync();

        if (await context.Users.AnyAsync(x => x.Id == request.DeliveredToId)) return Result.Failure(null);

        if (package is null)
            return Result.Failure(PackageErrors.PackageNotFoundOnMarkAsDelivered);

        if (package.Status == PackageStatus.DELIVERED) return Result.Failure(PackageErrors.PackageAlreadyDelivered);

        package.Status = PackageStatus.DELIVERED;
        package.DeliveredToId = request.DeliveredToId;
        package.DeliveredDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time"));
        await context.SaveChangesAsync();

        var packageDelivered = new PackageDeliveredCommand { DeliveredToId = request.DeliveredToId, Id = request.Id };
        _ = await mediator.Send(packageDelivered);

        return Result.Success();
    }
}