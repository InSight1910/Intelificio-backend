using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Queries.GetAllByUser
{
    public class GetAllByUserQuery : IRequest<Result>
    {
        public required int UserId { get; set; }
    }
}
