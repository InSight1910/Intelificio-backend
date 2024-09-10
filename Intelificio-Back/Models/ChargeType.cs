using Backend.Models.Base;

namespace Backend.Models
{
    public class ChargeType : BaseEntity
    {
        public required string Description { get; set; }

        public required IEnumerable<Charge> Charges { get; set; }
    }
}
