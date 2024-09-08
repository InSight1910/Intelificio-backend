using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class AssignedShift : BaseEntity
    {
        public int ShiftId { get; set; }
        public int StaffId { get; set; }
        public int CommunityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
