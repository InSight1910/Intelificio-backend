using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.CommonExpenses;
using Backend.Features.Notification.Common;
using Backend.Models;
using K4os.Compression.LZ4.Internal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Globalization;


namespace Backend.Features.Notification.Commands.Maintenance
{
    public class MaintenanceHandler : IRequestHandler<MaintenanceCommand, Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<MaintenanceHandler> _logger;
        private readonly IMapper _mapper;

        public MaintenanceHandler(IntelificioDbContext context, ILogger<MaintenanceHandler> logger, IMapper mapper, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(MaintenanceCommand request, CancellationToken cancellationToken)
        {
            var communityData = new
            {
                CommunityName = string.Empty,
                SenderAddress = string.Empty,
                CommonSpaceName = string.Empty,
                Recipients = new List<EmailAddress>()
            };

            // Caso 1: Filtrar por piso
            if (request.Floor > 0)
            {
                var commonSpace = await _context.CommonSpaces.Where(c => c.CommunityId == request.CommunityID).FirstOrDefaultAsync(c => c.ID == request.CommonSpaceID);
                if (commonSpace == null) return Result.Failure(NotificationErrors.CommonSpaceNotFound);

                communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = $"{ c.Address} {c.Municipality.Name}",
                        CommonSpaceName = commonSpace.Name,
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
                var commonSpace = await _context.CommonSpaces.Where(c => c.CommunityId == request.CommunityID).FirstOrDefaultAsync(c => c.ID == request.CommonSpaceID);
                if (commonSpace == null) return Result.Failure(NotificationErrors.CommonSpaceNotFound);

                communityData = await _context.Community
                    .Include(c => c.Buildings)
                    .ThenInclude(b => b.Units)
                    .ThenInclude(u => u.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = c.Address,
                        CommonSpaceName = commonSpace.Name,
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
                var commonSpace = await _context.CommonSpaces.Where(c => c.CommunityId == request.CommunityID).FirstOrDefaultAsync(c => c.ID == request.CommonSpaceID);
                if (commonSpace == null) return Result.Failure(NotificationErrors.CommonSpaceNotFound);

                communityData = await _context.Community
                    .Include(c => c.Users)
                    .Where(c => c.ID == request.CommunityID)
                    .Select(c => new
                    {
                        CommunityName = c.Name,
                        SenderAddress = c.Address,
                        CommonSpaceName = commonSpace.Name,
                        Recipients = c.Users
                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
            }

            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "Maintenance").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnMaintenance);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnMaintenance);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{communityData.CommunityName} {" a través de Intelificio"}");

            var templates = new List<MaintenanceTemplate>();

            string startDateString = request.StartDate;
            DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd", null);
            string endDateString = request.EndDate;
            DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd", null);

            string[] diasSemana = { "domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado" };
            string[] meses = { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };

            string dayOfWeekSpanish = diasSemana[(int)startDate.DayOfWeek];
            string monthSpanish = meses[startDate.Month - 1];

            foreach (var recipient in communityData.Recipients)
            {
                var template = new MaintenanceTemplate
                {
                    CommunityName = communityData.CommunityName,
                    ShortDate = $"{dayOfWeekSpanish} {startDate.Day} de {monthSpanish}",
                    AreaUnderMaintenance = communityData.CommonSpaceName,
                    StartDate = startDate.ToString("dd-MM-yyyy"),
                    EndDate = endDate.ToString("dd-MM-yyyy"),
                    SenderAddress = communityData.SenderAddress,
                };
                templates.Add(template);
            }

            if (templates == null) return Result.Failure(NotificationErrors.TemplateNotCreated);

            //var result = await _sendMail.SendSingleDynamicEmailToMultipleRecipientsAsync(template, templateNotification.TemplateId, from, communityData.Recipients);
            var result = await _sendMail.SendMaintenanceNotificationToMultipleRecipients(from, communityData.Recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnPackage);
            return Result.Success();

        }

    }
}
