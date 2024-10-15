namespace Backend.Features.Attendees.Queries.GetAttendeeByReservation;

public class GetAttendeeByReservationQueryResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Rut { get; set; }
    public required string Email { get; set; }
}