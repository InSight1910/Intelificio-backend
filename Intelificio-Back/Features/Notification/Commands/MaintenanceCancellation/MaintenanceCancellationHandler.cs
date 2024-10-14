using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Maintenance;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using SendGrid.Helpers.Mail;

namespace Backend.Features.Notification.Commands.MaintenanceCancellation
{
    public class MaintenanceCancellationHandler: IRequestHandler<MaintenanceCancellationCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<MaintenanceCancellationHandler> _logger;
        private readonly IMapper _mapper;

        public MaintenanceCancellationHandler(IntelificioDbContext context, ILogger<MaintenanceCancellationHandler> logger, IMapper mapper, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(MaintenanceCancellationCommand request, CancellationToken cancellationToken)
        {

            var commonSpace = await _context.CommonSpaces
                                            .FirstOrDefaultAsync(c => c.CommunityId == request.CommunityID && c.ID == request.CommonSpaceID, cancellationToken);

            if (commonSpace == null) return Result.Failure(NotificationErrors.CommonSpaceNotFoundOnMaintenanceCancellation);

            var communityData = await _context.Community
                                              .Include(c => c.Users)
                                              .Where(c => c.ID == request.CommunityID)
                                              .Select(c => new
                                              {
                                                  CommunityName = c.Name,
                                                  SenderAddress = $"{c.Address}, {c.Municipality.Name}",
                                                  CommonSpaceName = commonSpace.Name,
                                                  Recipients = c.Users
                                                      .Select(user => new EmailAddress(user.Email, user.ToString()))
                                                      .ToList()
                                              })
                                              .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (communityData == null)
            {
                return Result.Failure(NotificationErrors.CommunityNotFoundOnMaintenanceCancellation);
            }

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "MaintenanceCancellation").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnMaintenanceCancellation);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnMaintenanceCancellation);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{communityData.CommunityName} {" a través de Intelificio"}");

            var templates = new List<MaintenanceCancellationTemplate>();

            foreach (var recipient in communityData.Recipients)
            {
                var template = new MaintenanceCancellationTemplate
                {
                    CommunityName = communityData.CommunityName,
                    AreaUnderMaintenance = communityData.CommonSpaceName,
                    SenderAddress = communityData.SenderAddress,
                };
                templates.Add(template);
            }

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreatedOnMaintenanceCancellation);

            var result = await _sendMail.SendMaintenanceCancellationNotificationToMultipleRecipients(from, communityData.Recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnMaintenanceCancellation);
            return Result.Success();

        }    

    }
}
