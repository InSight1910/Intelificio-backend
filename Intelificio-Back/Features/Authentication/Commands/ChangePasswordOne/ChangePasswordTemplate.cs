namespace Backend.Features.Authentication.Commands.ChangePasswordOne
{
    public class ChangePasswordTemplate
    {
        public required string UserName { get; set; } // Nombre de usuario
        public required string ResetLink { get; set; } // URL para realizar el restablecimiento

    }
}
