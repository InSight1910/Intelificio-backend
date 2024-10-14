using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Packages.Create;

public class CreatePackageCommand : IRequest<Result>
{
    public required int RecipientId { get; set; }
    public required string TrackingNumber { get; set; }
    public required int ConciergeId { get; set; }
    public required int CommunityId { get; set; }
}