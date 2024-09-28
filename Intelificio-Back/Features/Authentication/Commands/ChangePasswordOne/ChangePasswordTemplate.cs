namespace Backend.Features.Authentication.Commands.ChangePasswordOne
{
    public class ChangePasswordTemplate
    {
        public required string UserName { get; set; }
        public required string ResetLink { get; set; }

    }
}
