using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Province : BaseEntity
    {
        public required string Name { get; set; }
        public required Municipality Municipality { get; set; }  
        public required Region Region { get; set; }  
    }
}
