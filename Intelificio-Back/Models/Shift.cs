
using Backend.Models.Base;

namespace Backend.Models
{
    public class Shift : BaseEntity
    {
        public TimeOnly ClockIn { get; set; }
        public TimeOnly ClockOut { get; set; }

        public ICollection<AssignedShift> AssignedShifts { get; set; } = new List<AssignedShift>();

        public required ShiftType Type { get; set; }
    }
}
