namespace Backend.Features.Reservations.Commands.Create;

public class CreateReservationCommandResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly StartDate { get; set; }
    public TimeOnly EndDate { get; set; }
    public int SpaceId { get; set; }
}