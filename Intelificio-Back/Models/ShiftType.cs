using Backend.Models.Base;

namespace Backend.Models
{
    public class ShiftType : BaseEntity
    {
        public required string Description { get; set; }

        public IEnumerable<Shift> Shifts { get; set; }
    }
}
