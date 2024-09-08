using Intelificio_Back.Models.Base;

namespace Intelificio_Back.Models
{
    public class Building : BaseEntity
    {
        public required string Name { get; set; }
        //public int CommunityId { get; set; }
        public bool IsActive { get; set; }
        public required Community Community { get; set; }
    }
}
