using Backend.Common.Response;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Command;

public class MarkAsDeliveredCommandHandler(IntelificioDbContext context)
    : IRequestHandler<MarkAsDeliveredCommand, Result>
{
    public async Task<Result> Handle(MarkAsDeliveredCommand request, CancellationToken cancellationToken)
    {
        var package = await context.Package
            .Include(x => x.Recipient)
            .Include(x => x.Concierge)
            .Include(x => x.DeliveredTo)
            .Where(x => x.ID == request.Id).FirstOrDefaultAsync();

        if (!await context.Users.AnyAsync(x => x.Id == request.DeliveredToId)) return Result.Failure(null);

        if (package is null)
            return Result.Failure(PackageErrors.PackageNotFoundOnMarkAsDelivered);

        if (package.Status == PackageStatus.DELIVERED) return Result.Failure(PackageErrors.PackageAlreadyDelivered);

        package.Status = PackageStatus.DELIVERED;
        package.DeliveredToId = request.DeliveredToId;
        package.DeliveredDate = DateTime.UtcNow;
        await context.SaveChangesAsync();


        return Result.Success();
    }
}