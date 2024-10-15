namespace Backend.Features.Packages.Command.Create;

public class CreatePackageCommandResponse
{
    public required int Id { get; set; }
    public required string TrackingNumber { get; set; }
    public required int Status { get; set; }
    public required string RecipientName { get; set; }
    public required string ConciergeName { get; set; }
    public required int NotificacionSent { get; set; }
    public required DateTime ReceptionDate { get; set; }
}