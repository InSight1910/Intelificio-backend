using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Packages.Queries.GetByCommunity;

public class GetByCommunityQuery : IRequest<Result>
{
    public required int CommunityId { get; set; }
}