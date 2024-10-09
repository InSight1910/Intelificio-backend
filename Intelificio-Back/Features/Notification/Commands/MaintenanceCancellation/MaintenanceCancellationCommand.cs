using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.MaintenanceCancellation
{
    public class MaintenanceCancellationCommand : IRequest<Result>
    {
        public required int CommunityID { get; set; }
        public required int CommonSpaceID { get; set; } = 0;
    }
}
