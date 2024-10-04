using Backend.Common.Response;
using MediatR;

namespace Backend.Features.CommonSpaces.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQuery : IRequest<Result>
    {
        public int CommunityId { get; set; }
    }
}
