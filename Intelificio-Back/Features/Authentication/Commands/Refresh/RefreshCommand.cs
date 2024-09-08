using Intelificio_Back.Common.Response;
using MediatR;

namespace Intelificio_Back.Features.Authentication.Commands.Refresh
{
    public class RefreshCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
