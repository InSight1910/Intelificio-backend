using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.ChangePasswordTwo
{
    public class ChangePasswordTwoCommand : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
    }
}
