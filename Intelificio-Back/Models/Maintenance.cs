using Backend.Models.Base;

namespace Backend.Models
{
    public class Maintenance : BaseEntity
    {
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public required int CommonSpaceID { get; set; }
        public  CommonSpace CommonSpace { get; set;}
        public bool IsActive { get; set; }
        public required int CommunityID { get; set; }
        public Community Community { get; set; }

    }
}
