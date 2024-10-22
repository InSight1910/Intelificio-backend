namespace Backend.Features.Notification.Commands.FineNotification
{
    public class FineNotificationTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string FineName { get; set; } // Nombre de la Multa | Ejemplo: "Ruidos Molestos"
        public required string UserName { get; set; } // Nombre del propietario de la unidad | Ejemplo: "Juan Carlos"
        public required string UnitName { get; set; } // Nombre de la unidad | Ejemplo: "201"
        public required string FineAmount { get; set; } // Monto de la multa | Ejemplo: "4 U.T.M | $86.999 aprox."
        public required string EventDate { get; set; } // Fecha de inicio | Ejemplo: 26-09-2024 15:00 PM"
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida
    }
}

