namespace Backend.Features.Reservations.Query.GetReservationsByUser;

public class GetReservationsByUserQueryResponse
{
    public int Id { get; set; }
    public string SpaceName { get; set; }
    public DateTime Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int Status { get; set; }
    public int Attendees { get; set; }
    public string Location { get; set; }
}