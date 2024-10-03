using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models;

public class Reservation : BaseEntity
{
    public required DateTime Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required ReservationStatus Status { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }
    public int SpaceId { get; set; }
    public virtual CommonSpace Spaces { get; set; }

    public ICollection<Invitee> Invites { get; set; } = new List<Invitee>();
}