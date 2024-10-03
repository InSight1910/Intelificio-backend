namespace Backend.Features.Authentication.Commands.UpdateUser
{
    public class UpdateUserResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
