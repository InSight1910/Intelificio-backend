using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Attendance: BaseEntity
    {   
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public DateTime Date { get; set; }
    }
}
