using Backend.Models.Base;

namespace Backend.Models
{
    public class Visit : BaseEntity
    {
        public required string Name { get; set; }
        public required string Rut { get; set; }
        public required string Plate { get; set; }

        public required User User { get; set; }

    }
}
