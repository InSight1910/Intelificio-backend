using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
{
    public class Package : BaseEntity
    {
        public required string Notes { get; set; }
        public required DateTime ReceptionDate { get; set; }
        public required PackageStatus Status { get; set; }
        public required Community Community { get; set; }
        public required int OwnerId { get; set; }
        public required User Owner { get; set; }
        public required int StaffId { get; set; }
        public required User Staff { get; set; }
    }

}
