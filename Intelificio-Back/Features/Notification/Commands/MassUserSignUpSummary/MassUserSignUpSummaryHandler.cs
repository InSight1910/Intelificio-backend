using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.SingleUserSignUpSummary;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.MassUserSignUpSummary
{
    public class MassUserSignUpSummaryHandler : IRequestHandler<MassUserSignUpSummaryCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<MassUserSignUpSummaryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;


        public MassUserSignUpSummaryHandler(IntelificioDbContext context, ILogger<MassUserSignUpSummaryHandler> logger, UserManager<User> userManager, IMapper mapper, SendMail sendMail)
        {
            _userManager = userManager;
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(MassUserSignUpSummaryCommand request, CancellationToken cancellationToken)
        {

            // Obtener el administrador de la comunidad usando el CreatorID
            var creator = await _context.Users.Where(u => u.Id == request.CreatorID).FirstOrDefaultAsync(cancellationToken);
            if (creator == null) return Result.Failure(NotificationErrors.AdminUserNotExistOnMassUserSignUpSummary);

            var community = await _context.Community
                .Include(c => c.Municipality)
                .ThenInclude(m => m.City)
                .Where(c => c.ID == request.CommunityID)
                .FirstOrDefaultAsync(cancellationToken);

            if (community is null) return Result.Failure(NotificationErrors.CommunityNotFoundOnMassUserSignUpSummary);

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "MassUserSignUpSummary").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnMassUserSignUpSummary);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnMassUserSignUpSummary);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{community.Name} {"a través de Intelificio"}");

            var template = new MassUserSignUpSummaryTemplate
            {
                CommunityName = community.Name,
                TotalCreados = request.TotalCreados,
                TotalEnviados = request.TotalEnviados,
                TotalErrores = request.TotalCreados,
                AdminName = creator.ToString(),
                SenderAddress = $"{community.Address}, {community.Municipality.Name}"
            };

            List<EmailAddress> recipients = new List<EmailAddress> { new EmailAddress(creator.Email, creator.ToString()) };

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            templateNotification.TemplateId,
                                                            from,
                                                            recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnMassUserSignUpSummary);
            return Result.Success();

        }
    }
}


