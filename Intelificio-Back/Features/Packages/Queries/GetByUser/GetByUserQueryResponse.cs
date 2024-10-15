using Backend.Models.Enums;

namespace Backend.Features.Packages.Queries.GetByUser;

public class GetByUserQueryResponse
{
    public int Id { get; set; }
    public string TrackingNumber { get; set; }
    public DateTime ReceptionDate { get; set; }
    public string ConciergeName { get; set; }
    public PackageStatus Status { get; set; }
    public string AssignedTo { get; set; } = "-";
}