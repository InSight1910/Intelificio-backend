namespace Backend.Features.Notification.Commands.MassUserConfirmationEmail
{
    public class MassUserConfirmationEmailTemplate
    {
        public required string UserName { get; set; } // Nombre de usuario
        public required string ConfirmLink { get; set; } // URL para realizar la confirmación de correo
        public required string CommunityName { get; set; } // nombre de comunidad
    }
}
