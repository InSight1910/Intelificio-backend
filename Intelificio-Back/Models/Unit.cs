using Backend.Models.Base;

namespace Backend.Models
{
    public class Unit : BaseEntity
    {
        public required string Number { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
        public required Building Building { get; set; }
        public required UnitType UnitType { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();



    }
}
