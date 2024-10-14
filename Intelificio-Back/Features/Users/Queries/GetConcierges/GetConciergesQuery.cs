using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Users.Queries.GetConcierges;

public class GetConciergesQuery : IRequest<Result>
{
    public int CommunityId { get; set; }
}