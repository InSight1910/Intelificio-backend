using Backend.Models.Base;

namespace Backend.Models
{
    public class Building : BaseEntity
    {
        public required string Name { get; set; }
        public required Community Community { get; set; }
        public required int Floors { get; set; }
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}
