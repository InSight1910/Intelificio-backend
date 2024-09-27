using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;


namespace Backend.Features.Notification.Commands.SingleMessage
{
    public class SingleMessageHandler: IRequestHandler<SingleMessageCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SingleMessageHandler> _logger;
        private readonly IMapper _mapper;

        public SingleMessageHandler(IntelificioDbContext context, ILogger<SingleMessageHandler> logger, IMapper mapper, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
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
                                                    User = c.Users.FirstOrDefault(u => u.Id == request.RecipientId)
                                                })
                                                .FirstOrDefaultAsync();

            var recipients = new List<EmailAddress>();

            recipients.Add(new EmailAddress(
                user.User.Email,
                $"{user.User.FirstName} {user.User.LastName}"
            ));

            var template = new SingleMessageTemplate
            {
                Subject = request.Subject, 
                PreHeader = request.PreHeader,
                Title = request.Title,
                CommunityName = user.CommunityName,
                Message = request.Message,
                SenderAddress = user.SenderAddress,
                SenderName = user.CommunityName + " a través de Intelificio"
            };

            var result = await _sendMail.SendEmailDinamycAsync(
                                                            template,
                                                            TemplatesEnum.SingleMessageIntelificioId,
                                                            user.CommunityName,
                                                            recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSent);
            return Result.Success();

        }

    }
}
