using Backend.Models.Base;

namespace Backend.Models
{
    public class City : BaseEntity
    {
        public required string Name { get; set; }
        public required int RegionId { get; set; }
        public required Region Region { get; set; }
        public required IEnumerable<Municipality> Municipalities { get; set; }
    }
}
