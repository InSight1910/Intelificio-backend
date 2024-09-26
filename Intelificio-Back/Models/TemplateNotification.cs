using Backend.Models.Base;

namespace Backend.Models
{
    public class TemplateNotification : BaseEntity
    {
        public required string Name { get; set; }

        public required string TemplateId { get; set; }
        public required string DynamicTemplateName { get; set; }

        public required string Subject { get; set; }

        public required string PreHeader { get; set; }

        public required string Title { get; set; }

        public required string Message { get; set; }

        public required string URL { get; set; }
    }
}
