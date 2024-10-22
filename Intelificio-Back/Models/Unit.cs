using Backend.Models.Base;

namespace Backend.Models
{
    public class Unit : BaseEntity
    {
        public required string Number { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
        public Building Building { get; set; }
        public required int BuildingId { get; set; }
        public UnitType UnitType { get; set; }
        public required int UnitTypeId { get; set; }

        public int UserId { get; set; }
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Guest> Guests { get; set; } = [];
        public ICollection<AssignedFine> AssignedFines { get; set; } = [];
    }
}
