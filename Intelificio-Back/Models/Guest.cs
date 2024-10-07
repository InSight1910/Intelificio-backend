using Backend.Models.Base;

namespace Backend.Models
{
    public class Guest : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Rut { get; set; }
        public required string Plate { get; set; }
        public required DateTime EntryTime { get; set; }
        public required int UnitId { get; set; }
        public  Unit Unit { get; set; } 

    }
}
