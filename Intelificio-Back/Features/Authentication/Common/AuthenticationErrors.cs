using Backend.Common.Response;

namespace Backend.Features.Authentication.Common
{
    public class AuthenticationErrors
    {
        public static Error InvalidParameters(IEnumerable<string> errors) => new Error(
            "Authentication.SignUp.InvalidParameters", "One or more validations failed", errors);
        public static Error AlreadyCreated = new Error(
            "Authentication.SignUp.AlreadyCreated", "User already exist", new List<string> { "The email is already registered in our system." });
        public static Error UserNotFound = new Error(
            "Authentication.LogIn.UserNotFound", "User does not exist", new List<string> { "The user does not exist in our system." });
        public static Error EmailNotConfirmed = new Error(
            "Authentication.LogIn.EmailNotConfirmed", "User have not confirm the email", new List<string> { "The email must be confirmed before log in." });
        public static Error UserBlocked = new Error(
            "Authentication.LogIn.UserBlocked", "User is lockout", new List<string> { "The account is block." });
        public static Error InvalidCredentials(IEnumerable<string> errors) => new Error(
            "Authentication.Login.InvalidCredentials", "Invalid Credentials", errors);
        public static Error RefreshTokenError(IEnumerable<string> errors) => new Error(
            "Authentication.Refresh.RefreshTokenError", "Error is an error with the Refresh Token", errors);
    }
}
