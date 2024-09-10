using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
