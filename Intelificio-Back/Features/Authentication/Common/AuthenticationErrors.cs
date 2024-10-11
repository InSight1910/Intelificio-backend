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

        public static Error AlreadyCreatedEmail(string email) => new Error
        {
            Code = "Authentication.SignUp.AlreadyCreated",
            Message = string.Format("El correo {0} ya se encuentra en nuestro sistema.", email)
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

        public static Error RefreshTokenError(IEnumerable<string> errors) => new Error
        {
            Code = "Authentication.Refresh.RefreshTokenError",
            Message = "Error is an error with the Refresh Token",
            Errors = errors
        };

        public static Error UpdateUserError(IEnumerable<string> errors) => new Error
        {
            Code = "Authentication.UpdateUser.UpdateUserError",
            Message = "Error is an error with the Refresh Token",
            Errors = errors
        };

        public static Error UserNotFoundGetByEmail = new Error
        {
            Code = "Authentication.GetUserByEmail.UserNotFound",
            Message = "El usuario indicado no existe en nuestros registros."
        };

        public static Error TemplateNotFoundOnChangePasswordOne = new()
        {
            Code = "Authentication.ChangePasswordOne.TemplateNotFoundOnChangePasswordOne",
            Message = "No se ha podido encontrar el Dynamic template."
        };
        public static Error TemplateIdIsNullOnChangePasswordOne = new()
        {
            Code = "Authentication.ChangePasswordOne.TemplateIdIsNullOnChangePasswordOne",
            Message = "El TempleID es nulo y se necesita para crear un Dynamic Email."
        };

        public static Error USerNotSendOnConfirmEmail = new()
        {
            Code = "Authentication.ConfirmEmail.EmailNotSendOnConfirmEmail",
            Message = "No existe un usuario registrado bajo este correo."
        };
        public static Error UserAlreadyConfirmThisEmailOnOnConfirmEmail = new()
        {
            Code = "Authentication.ConfirmEmail.UserAlreadyConfirmThisEmailOnOnConfirmEmail",
            Message = "El usuario ya ha confirmado este correo."
        };
        public static Error InvalidToken = new Error
        {
            Code = "Authentication.ConfirmEmail.InvalidToken",
            Message = "El token indicado es invalido."
        };


        public static Error RoleNotFound { get; internal set; }
        public static Error EmailNotSent { get; internal set; }

        public static List<Error> SignUpMassiveError(List<Error> errors) => errors;
    }
}
