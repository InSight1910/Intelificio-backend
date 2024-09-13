using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Queries.GetById
{
    public class GetByIdCommunityQuery : IRequest<Result>
    {
        public int id { get; set; }
    }
}
