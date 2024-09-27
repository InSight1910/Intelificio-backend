namespace Backend.Features.Notification.Commands.SingleMessage
{
    public class SingleMessageTemplate
    {
        public required string Subject { get; set; }
        public required string PreHeader { get; set; }
        public required string Title { get; set; }
        public required string CommunityName { get; set; }
        public required string Message { get; set; }
        public required string SenderName { get; set; }
        public required string CommunityFooter { get; set; }
        public required string SenderAddress { get; set; }

        public required string Contact { get; set; }


    }
}
