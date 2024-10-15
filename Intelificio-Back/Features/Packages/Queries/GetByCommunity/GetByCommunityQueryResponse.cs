using Backend.Common.Response;
using Backend.Models.Enums;
using MediatR;

namespace Backend.Features.Packages.Queries.GetByCommunity;

public class GetByCommunityQueryResponse
{
    public int Id { get; set; }
    public required string TrackingNumber { get; set; }
    public required string RecipientName { get; set; }
    public required string ConciergeName { get; set; }
    public DateTime ReceptionDate { get; set; }
    public required PackageStatus Status { get; set; }
    public required string DeliveredToName { get; set; }
    public string CanRetire { get; set; } = string.Empty;
    public int NotificacionSent { get; set; }
}