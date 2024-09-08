using Microsoft.AspNetCore.Identity;

namespace Intelificio_Back.Models
{
    public class Role : IdentityRole<int>
    {
        public required string Name { get; set; }
    }
}
