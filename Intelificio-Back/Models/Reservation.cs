using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models;

public class Reservation : BaseEntity
{
    public required DateTime Date { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
    public required ReservationStatus Status { get; set; }
    public string ConfirmationToken { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int SpaceId { get; set; }
    public virtual CommonSpace Spaces { get; set; }

    public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
}