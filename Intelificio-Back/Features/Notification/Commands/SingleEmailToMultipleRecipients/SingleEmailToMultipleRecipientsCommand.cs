using MediatR;
using Backend.Common.Response;


namespace Backend.Features.Notification.Commands.SingleEmailToMultipleRecipients
{
    public class SingleEmailToMultipleRecipientsCommand : IRequest<Result>
    {
        public required int TemplateNotificationId { get; set; }

        public required string StartDate { get; set; }

        public required string EndDate { get; set; }

        public required string ShortDate { get; set; }

        public required string AreaUnderMaintenance { get; set; }

        public required string SenderName { get; set; }

        public required string contact { get; set; }

        public required int CommunityID { get; set; }

        public int BuildingID { get; set; } = 0;
        public int Floor { get; set; } = 0;

    }
}
