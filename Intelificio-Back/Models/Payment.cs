using Intelificio_Back.Models.Base;
using Intelificio_Back.Models.Enums;

namespace Intelificio_Back.Models
{
    public class Payment : BaseEntity
    {
        public required int Amount { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required PaymentType PaymentType { get; set; }
        public required string URL { get; set; }
        public required Charge Charge { get; set; }
    }
}
