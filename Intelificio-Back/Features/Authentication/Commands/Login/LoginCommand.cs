using Intelificio_Back.Common.Response;
using MediatR;

namespace Intelificio_Back.Features.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
