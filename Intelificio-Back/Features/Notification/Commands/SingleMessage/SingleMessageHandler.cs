using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using SendGrid.Helpers.Mail;


namespace Backend.Features.Notification.Commands.SingleMessage
{
    public class SingleMessageHandler: IRequestHandler<SingleMessageCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SingleMessageHandler> _logger;

        public SingleMessageHandler(IntelificioDbContext context, ILogger<SingleMessageHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(SingleMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Community
                                                .Include(c => c.Users)
                                                .Where(c => c.ID == request.CommunityID)
                                                .Select(c => new
                                                {
                                                    CommunityName = c.Name ?? "",
                                                    SenderAddress = c.Address,
                                                    User = c.Users.FirstOrDefault(u => u.Id == request.RecipientId),
                                                })
                                                .FirstOrDefaultAsync();


            var recipients = new List<EmailAddress>();

            recipients.Add(new EmailAddress(
                user.User.Email ?? "intelificio@duocuc.cl",
                $"{user.User.FirstName} {user.User.LastName}"
            ));

            var template = new SingleMessageTemplate
            {
                Subject = request.Subject, 
                PreHeader = request.PreHeader,
                Title = request.Title,
                CommunityName = user.CommunityName,
                Message = request.Message,
                SenderAddress = user.SenderAddress
            };
            
            var from = new EmailAddress("intelificio@duocuc.cl", user.CommunityName + " a través de Intelificio");

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            TemplatesEnum.SingleMessageIntelificioId,
                                                            from,
                                                            recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSent);
            return Result.Success();

        }

    }
}
