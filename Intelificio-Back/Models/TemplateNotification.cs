using Backend.Models.Base;

namespace Backend.Models
{
    public class TemplateNotification : BaseEntity
    {
        public required string Name { get; set; }
        public required string TemplateId { get; set; }
        public required string DynamicTemplateName { get; set; }

    }
}
