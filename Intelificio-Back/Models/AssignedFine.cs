using Backend.Models.Base;

namespace Backend.Models
{
    public class AssignedFine : BaseEntity
    {
        public required int FineId { get; set; }

        public  Fine Fine { get; set; }

        public required int UnitId { get; set; }

        public  Unit Unit { get; set; }

        public DateTime EventDate { get; set; }

        public required string Comment { get; set; }

    }
}
