using Backend.Models.Base;
using Backend.Models.Enums;
using Intelificio_Back.Models;

namespace Backend.Models
{
    public class Reservation : BaseEntity
    {
        public int CommonAreaId { get; set; }
        public int UserId { get; set; }
        public required DateTime ReservationDate { get; set; }
        public required ReservationStatus Status { get; set; }

        public required User User { get; set; }

        public required CommonSpace Spaces { get; set; }

        public IEnumerable<Invitee> Invitees { get; set; } = Enumerable.Empty<Invitee>();
    }
}
