using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
{
    public class Package : BaseEntity
    {
        public required string Notes { get; set; }
        public required DateTime ReceptionDate { get; set; }
        public required PackageStatus Status { get; set; }
    }

}
