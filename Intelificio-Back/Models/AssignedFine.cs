using Backend.Models.Base;

namespace Backend.Models
{
    public class AssignedFine : BaseEntity
    {
        public int FineId { get; set; }

        public required Fine Fine { get; set; }

        public int UnitId { get; set; }

        public required Unit Unit { get; set; }

        public DateTime EventDate { get; set; }

        public required string Comment { get; set; }

    }
}
