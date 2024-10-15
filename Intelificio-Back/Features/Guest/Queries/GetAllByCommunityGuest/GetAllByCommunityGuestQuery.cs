using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Guest.Queries.GetAllByCommunity
{
    public class GetAllByCommunityGuestQuery : IRequest<Result>
    {
        public required int CommunityId { get; set; }
    }
}

