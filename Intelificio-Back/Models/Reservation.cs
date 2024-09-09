using Intelificio_Back.Models.Base;
using Intelificio_Back.Models.Enums;

namespace Intelificio_Back.Models
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
