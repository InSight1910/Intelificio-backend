using Backend.Models.Base;

namespace Backend.Models
{
    public class CommonSpace : BaseEntity
    {
        // falta ID_Comunidad
        public required string Name { get; set; }

        public int Capacity { get; set; }
        public required string AvailableHours { get; set; }

        public required Community Community { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
