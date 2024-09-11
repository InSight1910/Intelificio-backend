using Backend.Models.Base;

namespace Backend.Models
{
    public class ChargeType : BaseEntity
    {
        public required string Description { get; set; }

        public ICollection<Charge> Charges { get; set; } = new List<Charge>();
    }
}
