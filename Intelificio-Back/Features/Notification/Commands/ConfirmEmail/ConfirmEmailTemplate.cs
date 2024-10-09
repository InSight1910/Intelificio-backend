﻿namespace Backend.Features.Notification.Commands.ConfirmEmail
{
    public class ConfirmEmailTemplate
    {
        public required string UserName { get; set; } // Nombre de usuario
        public required string ConfirmLink { get; set; } // URL para realizar la confirmación de correo

    }
}
