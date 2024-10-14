using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.Refresh
{
    public class RefreshCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
