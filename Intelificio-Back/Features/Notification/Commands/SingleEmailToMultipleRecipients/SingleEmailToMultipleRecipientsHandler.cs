using Backend.Features.Notification.Commands.SingleMessage;
using MediatR;
using Backend.Common.Response;
using AutoMapper;
using Backend.Common.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Cms;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Features.Notification.Commands.SingleEmailToMultipleRecipients
{
    public class SingleEmailToMultipleRecipientsHandler : IRequestHandler<SingleEmailToMultipleRecipientsCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<SingleEmailToMultipleRecipientsHandler> _logger;
        private readonly IMapper _mapper;

        public SingleEmailToMultipleRecipientsHandler(IntelificioDbContext context, ILogger<SingleEmailToMultipleRecipientsHandler> logger, IMapper mapper, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(SingleEmailToMultipleRecipientsCommand request, CancellationToken cancellationToken)
        {
            var communityData = new
            {
                CommunityName = string.Empty,
                SenderAddress = string.Empty,
                Recipients = new List<EmailAddress>()
            };

            if (request.Floor > 0)
            {
                 communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users) // Relación con los usuarios a través de unidades
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name ?? "",
                        SenderAddress = c.Address,
                        Recipients = c.Buildings
                            .Where(b => b.ID == request.BuildingID) // Filtrar por el edificio
                            .SelectMany(b => b.Units)
                            .Where(u => u.Floor == request.Floor) // Filtrar por el piso
                            .SelectMany(u => u.Users)
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync();


            }
            else if (request.BuildingID > 0) // Caso 2: Enviar a usuarios de un edificio específico
            {
                 communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users) // Relación con los usuarios a través de unidades
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name ?? "",
                        SenderAddress = c.Address,
                        Recipients = c.Buildings
                            .Where(b => b.ID == request.BuildingID) // Filtrar por edificio
                            .SelectMany(b => b.Units)
                            .SelectMany(u => u.Users)
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
            }
            else if (request.BuildingID <= 0 && request.Floor <= 0)
            {
                 communityData = await _context.Community
                    .Include(c => c.Users) // Incluye los usuarios de la comunidad
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name ?? "",
                        SenderAddress = c.Address,
                        Recipients = c.Users
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
            }
                
            if (communityData == null || !communityData.Recipients.Any())
            {
                return Result.Failure("No se encontraron destinatarios para esta comunidad.");
            }

            var templateNotification = await _context.TemplateNotifications.Where(t => t.ID == request.TemplateNotificationId).FirstOrDefaultAsync();

            if (templateNotification == null)
            {
                throw new Exception("Template no encontrado.");
            }

            var replacements = new Dictionary<string, string>
{
                { "{{CommunityName}}", communityData.CommunityName },
                { "{{ShortDate}}", request.ShortDate },
                { "{{AreaUnderMaintenance}}", request.AreaUnderMaintenance },
                { "{{StartDate}}", request.StartDate },
                { "{{EndDate}}", request.EndDate },

            };

            string ReplacePlaceholders(string text, Dictionary<string, string> replacements)
            {
                foreach (var replacement in replacements)
                {
                    text = text.Replace(replacement.Key, replacement.Value);
                }
                return text;
            }

            var templateSendGrid = new SingleMessageTemplate
            {
                Subject = string.Empty,
                PreHeader = string.Empty,
                Title = string.Empty,
                CommunityName = string.Empty,
                Message = string.Empty,
                SenderAddress = string.Empty, 
                SenderName = string.Empty,
                CommunityFooter = string.Empty,
                Contact = string.Empty,
            };

            if (request.TemplateNotificationId == 1)
            {
                 templateSendGrid = new SingleMessageTemplate
                {
                    Subject = ReplacePlaceholders(templateNotification.Subject, replacements),
                    PreHeader = ReplacePlaceholders(templateNotification.PreHeader, replacements),
                    Title = ReplacePlaceholders(templateNotification.Title, replacements),
                    CommunityName = communityData.CommunityName,
                    Message = ReplacePlaceholders(templateNotification.Message, replacements),
                    SenderAddress = communityData.SenderAddress,
                    SenderName = request.SenderName,
                    CommunityFooter = communityData.CommunityName + " a través de Intelificio",
                    Contact = request.contact,
                 };
            }

            var result = await _sendMail.SendEmailDinamycAsync(
                templateSendGrid,
                templateNotification.TemplateId,
                communityData.CommunityName,
                communityData.Recipients
            );

            if (!result.IsSuccessStatusCode)
            {
                return Result.Failure("Error al enviar el correo.");
            }

            return Result.Success();
            }
        }
    }
