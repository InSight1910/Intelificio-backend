using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class UnitType : BaseEntity
    {
        public required string Description {  get; set; }
        public required IEnumerable<Unit> Units { get; set; }
    }
}