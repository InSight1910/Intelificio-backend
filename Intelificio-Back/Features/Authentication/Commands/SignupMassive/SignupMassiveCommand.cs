using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.SignupMassive
{
    public class SignupMassiveCommand : IRequest<Result>
    {
        public required IFormFile File { get; set; }
    }
}
