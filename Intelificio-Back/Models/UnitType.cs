using Backend.Models.Base;

namespace Backend.Models
{
    public class UnitType : BaseEntity
    {
        public required string Description { get; set; }
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
    }
}