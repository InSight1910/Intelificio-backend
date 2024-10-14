namespace Backend.Features.Notification.Commands.Reservation.ReservationConfirmation
{
    public class ConfirmReservationEmailTemplate

    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string CommonSpaceName { get; set; } // Nombre de espacio común | Ejemplo: Sala de estar
        public required string UserName { get; set; } // Nombre de usuario
        public required string ConfirmLink { get; set; } // URL para realizar el restablecimiento

    }
}
