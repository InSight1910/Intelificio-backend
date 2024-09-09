using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class CommonSpace: BaseEntity
    {
        // falta ID_Comunidad
        public required string Name { get; set; }
       
        public int Capacity { get; set; }
        public required string AvailableHours { get; set; }

        public required Community Community { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; } = Enumerable.Empty<Reservation>();
    }
}
