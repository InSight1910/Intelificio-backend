using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Notification.Commands.SingleMessage
{
    public class SingleMessageCommand : IRequest<Result>
    {
        public required string Subject { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string SenderName { get; set; }
        public required int CommunityID { get; set; }
        public int BuildingID { get; set; } = 0;
        public int Floor { get; set; } = 0;

    }
}
