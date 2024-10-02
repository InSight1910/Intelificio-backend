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
            var communityData = new
            {
                CommunityName = string.Empty,
                SenderAddress = string.Empty,
                Recipients = new List<EmailAddress>()
            };

            // Caso 1: Filtrar por piso
            if (request.Floor > 0 && request.BuildingID > 0)
            {
                communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = $"{c.Address} {c.Municipality.Name}",
                        Recipients = c.Buildings
                            .Where(b => b.ID == request.BuildingID)
                            .SelectMany(b => b.Units)
                            .Where(u => u.Floor == request.Floor)
                            .SelectMany(u => u.Users)
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }
            // Caso 2: Filtrar por edificio
            else if (request.BuildingID > 0)
            {

                communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = c.Address,
                        Recipients = c.Buildings
                            .Where(b => b.ID == request.BuildingID)
                            .SelectMany(b => b.Units)
                            .SelectMany(u => u.Users)
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }
            // Caso 3: Filtrar por comunidad completa
            else
            {

                communityData = await _context.Community
                    .Include(c => c.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = c.Address,
                        Recipients = c.Users
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
            }

            var templateNotification = await _context.TemplateNotifications.Where(t => t.ID == 7).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnSimpleMessage);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnSimpleMessage);

            var template = new SingleMessageTemplate
            {
                Subject = request.Subject, 
                Title = request.Title,
                CommunityName = communityData.CommunityName ?? " ",
                Message = request.Message,
                SenderName = request.SenderName,
                SenderAddress = communityData.SenderAddress
            };
            
            var from = new EmailAddress("intelificio@duocuc.cl", communityData.CommunityName + " a través de Intelificio");

            var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(
                                                            template,
                                                            templateNotification.TemplateId,
                                                            from,
                                                            communityData.Recipients
                                                        );

            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSent);
            return Result.Success();

        }

    }
}
