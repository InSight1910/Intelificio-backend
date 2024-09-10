using Backend.Models.Base;

namespace Backend.Models
{
    public class Attendance : BaseEntity
    {
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public DateTime Date { get; set; }

        public required User User { get; set; }
    }
}
