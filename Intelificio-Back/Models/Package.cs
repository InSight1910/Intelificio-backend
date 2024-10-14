using Backend.Models.Base;
using Backend.Models.Enums;
using System;

namespace Backend.Models;

public class Package : BaseEntity
{
    public required string TrackingNumber { get; set; }
    public DateTime ReceptionDate { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time"));
    public required PackageStatus Status { get; set; }
    public required int CommunityId { get; set; }
    public Community Community { get; set; }
    public required int RecipientId { get; set; }
    public User Recipient { get; set; }
    public required int ConciergeId { get; set; }
    public User Concierge { get; set; }
    public int? DeliveredToId { get; set; }
    public User? DeliveredTo { get; set; }
    public DateTime? DeliveredDate { get; set; }

    public DateTime NotificationDate { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time"));
    public int NotificacionSent { get; set; } = 0;
}

