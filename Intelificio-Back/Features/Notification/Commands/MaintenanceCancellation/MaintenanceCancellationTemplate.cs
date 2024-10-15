namespace Backend.Features.Notification.Commands.MaintenanceCancellation
{
    public class MaintenanceCancellationTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida

        public required string AreaUnderMaintenance { get; set; } // Espacio comun en mantención | Ejemplo: Piscina
    }
}
