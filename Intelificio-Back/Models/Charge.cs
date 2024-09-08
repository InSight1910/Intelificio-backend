using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Charge : BaseEntity
    {
        public int Amount { get; set; }
        public bool IsFine { get; set; }
        public DateTime ChargeDate { get; set; }
        public bool IsActive { get; set; }

        public required ChargeType Type { get; set; }
    }
}
