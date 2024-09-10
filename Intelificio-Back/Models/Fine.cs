using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models

{
    public class Fine : BaseEntity
    {
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }

    }
}
