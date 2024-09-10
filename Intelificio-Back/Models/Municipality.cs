using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Municipality : BaseEntity
    {
        public required string Name { get; set; }
        //public int ProvinceId { get; set; }
        public required Community Community { get; set; } 
        public required Province  Province { get; set; }
    } 
}
