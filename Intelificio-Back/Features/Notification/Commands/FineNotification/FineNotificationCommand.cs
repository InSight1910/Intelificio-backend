using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.FineNotification
{
    public class FineNotificationCommand: IRequest<Result>
    {
        public int AssignedFineId { get; set; }
    }
}
