using Backend.Models.Base;

namespace Backend.Models
{
    public class Unit : BaseEntity
    {
        //public UnitType Type { get; set; }
        public int Number { get; set; }
        //public int UserId { get; set; }
        //public UnitState State { get; set; }
        public bool IsActive { get; set; }

    }
}
