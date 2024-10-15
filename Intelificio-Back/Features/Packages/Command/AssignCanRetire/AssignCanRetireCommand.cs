using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Packages.Command.AssignCanRetire;

public class AssignCanRetireCommand : IRequest<Result>
{
    public int PackageId { get; set; }
    public int UserId { get; set; }
    public int CommunityId { get; set; }
}