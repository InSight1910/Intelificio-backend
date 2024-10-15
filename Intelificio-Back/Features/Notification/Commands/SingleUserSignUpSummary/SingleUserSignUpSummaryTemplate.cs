namespace Backend.Features.Notification.Commands.SingleUserSignUpSummary
{
    public class SingleUserSignUpSummaryTemplate
    {
        public required string CommunityName { get; set; } // Nombre de comunidad | Ejemplo: Las Brisas de San Juan
        public required string UserName { get; set; } // Nombre del usuario Creado
        public required string UserEmail { get; set; } // correo del usuario Creado
        public required string CreationDate { get; set; } // fecha Cuando se creo | DD-MM-YYYY en texto
        public required string AdminName { get; set; } // Nombre del administrador (Quien creo la cuenta en sistema)
        public required string SenderAddress { get; set; } // Dirección de la comunidad | Ejemplo: Tangamandapio #243, La Florida

    }
}
