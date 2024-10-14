namespace Backend.Features.Packages.Create;

public class CreatePackageCommandResponse
{
    public required int Id { get; set; }
    public required string TrackingNumber { get; set; }
    public required int Status { get; set; }
    public required string RecipientName { get; set; }
    public required string ConciergeName { get; set; }
    public required DateTime ArrivalDate { get; set; }
}