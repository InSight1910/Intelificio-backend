namespace Backend.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailUserCommandResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
