using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Visit : BaseEntity
    {
        public required string Name { get; set; }
        public required string Rut { get; set; }
        public required string Plate { get; set; }

        public required User User { get; set; }

    }
}
