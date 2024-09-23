using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.ChangePasswordOne
{
    public class ChangePasswordOneCommand : IRequest<Result>
    {
        public required string Email { get; set; }
    }
}
