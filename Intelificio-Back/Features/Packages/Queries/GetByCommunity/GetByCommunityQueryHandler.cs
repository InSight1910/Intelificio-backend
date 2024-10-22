using Backend.Common.Response;
using Backend.Features.Packages.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace Backend.Features.Packages.Queries.GetByCommunity;

public class GetByCommunityQueryHandler(IntelificioDbContext context) : IRequestHandler<GetByCommunityQuery, Result>
{
    public async Task<Result> Handle(GetByCommunityQuery request, CancellationToken cancellationToken)
    {
        var community = await context.Community.Where(x => x.ID == request.CommunityId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (community is null) return Result.Failure(PackageErrors.CommunityNotFoundOnGetByCommunityQuery);
        
        var currentDate =
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time"));

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
                ReceptionDate = TimeZoneInfo.ConvertTimeFromUtc(x.ReceptionDate, TZConvert.GetTimeZoneInfo(x.Community.TimeZone)),
                Status = x.Status,
                TrackingNumber = x.TrackingNumber,
                DeliveredToName = x.DeliveredTo != null ? x.DeliveredTo.ToString() : "-",
                CanRetire = x.CanRetire != null ? x.CanRetire.ToString() : "-",
                NotificacionSent = x.NotificacionSent,
                NotificationDate = TimeZoneInfo.ConvertTimeFromUtc(x.NotificationDate, TZConvert.GetTimeZoneInfo(x.Community.TimeZone)),
                CanSend = false,
            })
            .OrderByDescending(x => x.Status).ToListAsync();

        foreach (var package in result)
            if ((currentDate - TimeZoneInfo.ConvertTimeFromUtc(package.NotificationDate, TZConvert.GetTimeZoneInfo(community.TimeZone))).TotalHours >= 24 && package.Status == PackageStatus.PENDING)
                package.CanSend = true;

        return Result.WithResponse(new ResponseData
        {
            Data = result
        });
    }
}