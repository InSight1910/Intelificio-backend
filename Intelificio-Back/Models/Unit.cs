using Backend.Models.Base;

namespace Backend.Models
{
    public class Unit : BaseEntity
    {
        public required string Number { get; set; }
        public bool IsActive { get; set; }
        public  Building Building { get; set; }
        public  UnitType Type { get; set; }
        public ICollection<User> users { get; set; } = new List<User>();
    }
}
