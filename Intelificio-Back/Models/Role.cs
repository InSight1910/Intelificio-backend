using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class Role : IdentityRole<int>
    {
        public required string Name { get; set; }
    }
}
