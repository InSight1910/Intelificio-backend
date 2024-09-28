namespace Backend.Features.Notification.Commands.Maintenance
{
    public class MaintenanceTemplate 
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string ShortDate { get; set; } // Fecha Corta | Ejemplo: Martes 26 de Septiembre
        public required string AreaUnderMaintenance { get; set; } // Espacio comun en mantención | Ejemplo: Piscina
        public required string StartDate { get; set; } // Fecha de inidicio | Ejemplo: 26-09-2024 10:00 AM
        public required string EndDate { get; set; } // Fecha de inicio | Ejemplo: 26-09-2024 15:00 PM"
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida
    }
}
