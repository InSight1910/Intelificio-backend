using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Packages.Queries.GetByUser;

public class GetByUserQuery : IRequest<Result>
{
    public int CommunityId { get; set; }
    public int UserId { get; set; }
}