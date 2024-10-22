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
        public int? UserId { get; set; } // Id del usuario principal
        public User? User { get; set; } // Propiedad de navegación 
        public ICollection<User> Users { get; set; } = new List<User>(); // Colección de usuarios
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    }
}
