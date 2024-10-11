namespace Backend.Features.Notification.Commands.MassUserSignUpSummary
{
    public class MassUserSignUpSummaryTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string TotalCreados { get; set; } // Total de usuarios Creados
        public required string TotalEnviados { get; set; } // Total de usuarios enviados
        public required string TotalErrores { get; set; } // Total de usuarios no creados
        public required string AdminName { get; set; } // Nombre del administrador (Quien creo la cuenta en sistema)
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida
    }
}
