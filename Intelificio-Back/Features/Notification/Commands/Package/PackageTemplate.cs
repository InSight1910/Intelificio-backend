namespace Backend.Features.Notification.Commands.Package
{
    public class PackageTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string Day { get; set; } // Fecha Corta | Ejemplo: 27-09-2024
        public required string Hour { get; set; } // Hora de recepción | Ejemplo: 15:00 PM
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida
        public required string Name { get; set; } // Nombre del propietario | Ejemplo: Juan
    }
}
