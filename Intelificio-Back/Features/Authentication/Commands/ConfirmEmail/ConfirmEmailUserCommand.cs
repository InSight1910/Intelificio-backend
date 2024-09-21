using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailUserCommand : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
