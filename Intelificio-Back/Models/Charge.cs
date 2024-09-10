using Backend.Models.Base;

namespace Backend.Models
{
    public class Charge : BaseEntity
    {
        public int Amount { get; set; }
        public bool IsFine { get; set; }
        public DateTime ChargeDate { get; set; }
        public bool IsActive { get; set; }
        public required ChargeType Type { get; set; }
        public required Community Community { get; set; }
        public required User User { get; set; }
    }
}
