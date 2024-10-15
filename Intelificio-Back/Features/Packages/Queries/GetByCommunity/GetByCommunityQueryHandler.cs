using Backend.Common.Response;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Packages.Queries.GetByCommunity;

public class GetByCommunityQueryHandler(IntelificioDbContext context) : IRequestHandler<GetByCommunityQuery, Result>
{
    public async Task<Result> Handle(GetByCommunityQuery request, CancellationToken cancellationToken)
    {
        if (!await context.Community.AnyAsync(x => x.ID == request.CommunityId)) return Result.Failure(null);

        var currentDate =
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Santiago"));

        var result = await context.Package
            .Include(x => x.Concierge)
            .Include(x => x.Recipient)
            .Include(x => x.DeliveredTo)
            .Include(x => x.CanRetire)
            .Where(x => x.CommunityId == request.CommunityId)
            .Select(x => new GetByCommunityQueryResponse
            {
                Id = x.ID,
                ConciergeName = x.Concierge.ToString(),
                RecipientName = x.Recipient.ToString(),
                ReceptionDate = x.ReceptionDate,
                Status = x.Status,
                TrackingNumber = x.TrackingNumber,
                DeliveredToName = x.DeliveredTo != null ? x.DeliveredTo.ToString() : "-",
                CanRetire = x.CanRetire != null ? x.CanRetire.ToString() : "-",
                NotificacionSent = x.NotificacionSent,
                NotificationDate = x.NotificationDate,
                CanSend = false,
            })
            .OrderByDescending(x => x.Status).ToListAsync();

        foreach (var package in result)
            if ((currentDate - package.NotificationDate).TotalHours >= 24 && package.Status == PackageStatus.PENDING)
                package.CanSend = true;

        return Result.WithResponse(new ResponseData
        {
            Data = result
        });
    }
}