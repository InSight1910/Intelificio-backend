using Backend.Models.Base;

namespace Backend.Models
{
    public class City : BaseEntity
    {
        public required string Name { get; set; }
        public required Region Region { get; set; }
        public ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
    }
}
