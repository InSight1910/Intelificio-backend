using Backend.Models.Base;

namespace Backend.Models
{
    public class Municipality : BaseEntity
    {
        public required string Name { get; set; }
        public required City City { get; set; }
    }
}
