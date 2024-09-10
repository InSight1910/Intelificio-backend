using Backend.Models.Base;

namespace Backend.Models
{
    public class Contact : BaseEntity
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Service { get; set; }
        public required Community Community { get; set; }
    }
}
