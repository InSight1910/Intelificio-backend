using Backend.Models.Base;

namespace Backend.Models
{
    public class Region : BaseEntity
    {
        public required string Name { get; set; }
        public required IEnumerable<City> Cities { get; set; }
    }
}
