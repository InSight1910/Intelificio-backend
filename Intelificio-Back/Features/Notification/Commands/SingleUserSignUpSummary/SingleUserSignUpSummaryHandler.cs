using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.SingleUserSignUpSummary
{
    public class SingleUserSignUpSummaryHandler : IRequestHandler<SingleUserSignUpSummaryCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SingleUserSignUpSummaryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public SingleUserSignUpSummaryHandler(IntelificioDbContext context, ILogger<SingleUserSignUpSummaryHandler> logger, UserManager<User> userManager, IMapper mapper, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(SingleUserSignUpSummaryCommand request, CancellationToken cancellationToken)
        {
            // Obtener el administrador de la comunidad usando el CreatorID
            var creator = await _context.Users.Where(u => u.Id == request.CreatorID).FirstOrDefaultAsync(cancellationToken);
            if (creator == null) return Result.Failure(NotificationErrors.AdminUserNotExistOnSingleUserSignUpSummary);

            var community = await _context.Community
                .Include(c => c.Municipality)
                .ThenInclude(m => m.City)
                .Where(c => c.ID == request.CommunityID)
                .FirstOrDefaultAsync(cancellationToken);

            if (community is null) return Result.Failure(NotificationErrors.CommunityNotFoundOnSingleUserSignUpSummary);


            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "SingleUserSignUpSummary").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnSingleUserSignUpSummary);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnSingleUserSignUpSummary);


            var from = new EmailAddress("intelificio@duocuc.cl", $"{community.Name} {"a través de Intelificio"}");

            var template = new SingleUserSignUpSummaryTemplate
            {
                CommunityName = community.Name,
                UserName = $"{request.user.FirstName} {request.user.LastName}",
                UserEmail = request.user.Email ?? "Sin_Email",
                CreationDate = DateTime.Now.ToString("dd-MM-yyyy"),
                AdminName = $"{creator.FirstName} {creator.LastName}",
                SenderAddress = $"{community.Address}, {community.Municipality.Name}"
            };

            List<EmailAddress> recipients = new List<EmailAddress> { new EmailAddress(creator.Email, creator.ToString()) };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            templateNotification.TemplateId,
                                                            from,
                                                            recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnSingleUserSignUpSummary);
            return Result.Success();

        }

    }
}
