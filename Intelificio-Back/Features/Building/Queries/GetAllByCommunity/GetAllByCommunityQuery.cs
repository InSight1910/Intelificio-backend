using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Building.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQuery : IRequest<Result>
    {
        public required int CommunityId { get; set; }
    }
    
}
