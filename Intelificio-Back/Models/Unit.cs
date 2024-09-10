using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Unit : BaseEntity
    {
        //public UnitType Type { get; set; }
        public int Number { get; set; }
        //public int UserId { get; set; }
        //public UnitState State { get; set; }
        public bool IsActive { get; set; }
        public required Building Building { get; set; }
        public required UnitType Type { get; set; }
    }
}
