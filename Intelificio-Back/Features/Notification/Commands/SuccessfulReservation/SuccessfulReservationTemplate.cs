namespace Backend.Features.Notification.Commands.SuccessfulReservation
{
    public class SuccessfulReservationTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string CommonSpaceName { get; set; } // Nombre de espacio común | Ejemplo: Las brisas
        public required string Capacitu { get; set; } // Aforo maximo | Ejemplo: 10
        public required string Name { get; set; } // Nombre del usuario 
        public required string StartDate { get; set; } // Inicio de reserva | Ejemplo: 27-09-2024
        public required string EndDate { get; set; } // Fin de servava | Ejemplo: 15:00 PM
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida
    }
}
