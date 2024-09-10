using Intelificio_Back.Models.Base;
using Intelificio_Back.Models.Enums;
namespace Intelificio_Back.Models

{
    public class Fine : BaseEntity
    {
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }
        public required Charge Charge { get; set; }

    }
}
