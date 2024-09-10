using Backend.Models.Base;

namespace Backend.Models
{
    public class Province : BaseEntity
    {
        public required string Name { get; set; }
        public required Municipality Municipality { get; set; }  
        public required Region Region { get; set; }  
    }
}
