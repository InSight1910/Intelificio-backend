using Backend.Models.Base;

namespace Backend.Models
{
    public class Invitee : BaseEntity
    {
        public required string Rut { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
