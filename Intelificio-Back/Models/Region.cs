using Backend.Models.Base;

namespace Backend.Models
{
    public class Region : BaseEntity
    {
        public required string Name { get; set; }
        public required Province Province { get; set; }

    }
}
