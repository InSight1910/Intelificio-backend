using Backend.Models.Base;

namespace Backend.Models
{
    public class Region : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
