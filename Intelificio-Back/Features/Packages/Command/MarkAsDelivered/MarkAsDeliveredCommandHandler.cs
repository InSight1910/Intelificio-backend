using Backend.Common.Response;
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
        var package = await context.Package.Where(x => x.ID == request.Id).FirstOrDefaultAsync();

        if (await context.Users.AnyAsync(x => x.Id == request.DeliveredToId)) return Result.Failure(null);

        if (package is null)
            return Result.Failure(null);

        if (package.Status == PackageStatus.DELIVERED) return Result.Failure(null);

        package.Status = PackageStatus.DELIVERED;
        package.DeliveredToId = request.DeliveredToId;
        package.DeliveredDate = DateTime.UtcNow;

        return Result.Success();
    }
}