
using Backend.Models.Base;

namespace Backend.Models
{
    public class Shift : BaseEntity
    {
        public TimeOnly ClockIn { get; set; }
        public TimeOnly ClockOut { get; set; }

        public required IEnumerable<AssignedShift> AssignedShifts { get; set; }

        public required ShiftType Type { get; set; }
    }
}
