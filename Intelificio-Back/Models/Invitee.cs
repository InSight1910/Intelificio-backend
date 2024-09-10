using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Invitee : BaseEntity
    {
        public required string Rut { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; } = Enumerable.Empty<Reservation>();

    }
}
