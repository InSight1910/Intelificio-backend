using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Users.GetConcierges;

public class GetConciergesQuery : IRequest<Result>
{
    public int CommunityId { get; set; }
}