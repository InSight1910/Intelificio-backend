using Backend.Models.Base;
using Backend.Models.Enums;

namespace Backend.Models
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
