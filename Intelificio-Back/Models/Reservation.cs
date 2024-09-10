using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
{
    public class Reservation : BaseEntity
    {
        public int CommonAreaId { get; set; }
        public int UserId { get; set; }
        public required DateTime ReservationDate { get; set; }
        public required ReservationStatus Status { get; set; }

    }
}
