using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class ChargeType: BaseEntity
    {
        public required string Description { get; set; }

        public required IEnumerable<Charge> Charges { get; set; }
    }
}
