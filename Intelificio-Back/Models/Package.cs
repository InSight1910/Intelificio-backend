using Intelificio_Back.Models.Base;
using Intelificio_Back.Models.Enums;

namespace Intelificio_Back.Models
{
    public class Package : BaseEntity
    {
        public required string Notes { get; set; }
        public required DateTime ReceptionDate { get; set; }
        public required PackageStatus Status { get; set; }
        public required Community Community { get; set; }
    }

}
