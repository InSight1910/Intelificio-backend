using Backend.Models;
using Backend.Models.Base;

namespace Intelificio_Back.Models
{
    public class Invitee : BaseEntity
    {
        public required string Rut { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
