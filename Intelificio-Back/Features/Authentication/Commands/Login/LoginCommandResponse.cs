namespace Backend.Features.Authentication.Commands.Login
{
    public class LoginCommandResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }

    }
}
