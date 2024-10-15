using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.PackageDelivered
{
    public class PackageDeliveredCommand : IRequest<Result>
    {
        public required int Id { get; set; }
        public required int DeliveredToId { get; set; }
    }
}
