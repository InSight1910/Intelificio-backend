using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Queries.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<Result>
    {
        public required string Email { get; set; }
    }
}
