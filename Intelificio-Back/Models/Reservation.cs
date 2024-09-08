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

    }
}
