namespace Backend.Features.Authentication.Commands.Refresh
{
    public class RefreshCommandResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
