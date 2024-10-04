using Backend.Models.Base;
using Org.BouncyCastle.Asn1.Crmf;

namespace Backend.Models
{
    public class CommonSpace : BaseEntity
    {
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public required string Location { get; set; }
        public bool IsInMaintenance { get; set; }
        public int CommunityId { get; set; }

        public required Community Community { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
