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

        public required IEnumerable<User> Users { get; set; }

        public required Shift Shift { get; set; }

        public IEnumerable<Community> Communities { get; set; } = Enumerable.Empty<Community>();
    }
}
