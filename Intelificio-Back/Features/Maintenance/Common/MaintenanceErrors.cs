using Backend.Common.Response;

namespace Backend.Features.Maintenance.Common
{
    public class MaintenanceErrors
    {
        public static Error CommunityNotFoundOnQuery = new()
        {
            Code = "Maintenance.query.CommunityNotFoundOnQuery",
            Message = "No existe la comunidad consultada."
        };

        public static Error MaintenanceNotFoundOnQuery = new()
        {
            Code = "Maintenance.query.MaintenanceNotFoundOnQuery",
            Message = "No existe la mantención consultada."
        };
    }
}
