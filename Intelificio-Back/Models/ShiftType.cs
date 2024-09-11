using Backend.Models.Base;

namespace Backend.Models
{
    public class ShiftType : BaseEntity
    {
        public required string Description { get; set; }

        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
