using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.ConfirmEmailTwo
{
    public class ConfirmEmailTwoCommand : IRequest<Result>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
