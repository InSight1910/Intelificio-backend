using Backend.Common.Response;

namespace Backend.Features.Authentication.Common
{
    public class AuthenticationErrors
    {
        public static Error InvalidParameters(IEnumerable<string> errors) => new Error(
            "Authentication.SignUp.InvalidParameters", "One or more validations failed", errors);
        public static Error AlreadyCreated = new Error
        {
            Code = "Authentication.SignUp.AlreadyCreated",
            Message = "El correo indicado ya se encuentra en nuestro sistema."
        };
        public static Error UserNotFound = new Error
        {
            Code = "Authentication.LogIn.UserNotFound",
            Message = "El usuario indicado no existe en nuestros registros."
        };
        public static Error EmailNotConfirmed = new Error
        {
            Code = "Authentication.LogIn.EmailNotConfirmed",
            Message = "Se debe confirmar el correo electronico."
        };
        public static Error UserBlocked = new Error
        {
            Code = "Authentication.LogIn.UserBlocked",
            Message = "El usuario se encuentra bloqueado"
        };

        public static Error WrongPassword = new Error
        {
            Code = "Authentication.LogIn.WrongPassword",
            Message = "La contraseña indicada es erronea."
        };

        public static Error UserNotFoundConfirmEmail = new Error
        {
            Code = "Authentication.ConfirmEmail.UserNotFound",
            Message = "El usuario indicado no existe en nuestros registros."
        };
        public static Error InvalidToken = new Error
        {
            Code = "Authentication.ConfirmEmail.InvalidToken",
            Message = "El token indicado es invalido."
        };

        public static Error RefreshTokenError(IEnumerable<string> errors) => new Error(
            "Authentication.Refresh.RefreshTokenError", "Error is an error with the Refresh Token", errors);
    }
}
