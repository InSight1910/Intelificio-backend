using Intelificio_Back.Models.Base;
namespace Intelificio_Back.Models
{
    public class Maintenance : BaseEntity
    {
        public DateTime PostDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public required Community Community { get; set; }
        public required Building Building { get; set; }
    }
}
