using Backend.Models.Base;

namespace Backend.Models
{
    public class AssignedShift : BaseEntity
    {
        public int ShiftId { get; set; }
        public int StaffId { get; set; }
        public int CommunityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        public required Shift Shift { get; set; }

        public ICollection<Community> Communities { get; set; } = new List<Community>();
    }
}
