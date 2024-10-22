using AutoMapper;
using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;


namespace Backend.Features.Notification.Commands.Maintenance
{
    public class MaintenanceHandler(IntelificioDbContext context, ILogger<MaintenanceHandler> logger, IMapper mapper, SendMail sendMail) : IRequestHandler<MaintenanceCommand, Result>
    {
        private readonly SendMail _sendMail = sendMail;
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<MaintenanceHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(MaintenanceCommand request, CancellationToken cancellationToken)
        {

            DateTime startDate = DateTime.ParseExact(request.StartDate, "yyyy-MM-dd", null);
            DateTime endDate = DateTime.ParseExact(request.EndDate, "yyyy-MM-dd", null);


            var commonSpace = await _context.CommonSpaces.Where(c => c.CommunityId == request.CommunityID).FirstOrDefaultAsync(c => c.ID == request.CommonSpaceID, cancellationToken);
            if (commonSpace == null) return Result.Failure(NotificationErrors.CommonSpaceNotFound);

            var communityData = await _context.Community
                .Include(c => c.Users)
                .Where(c => c.ID == request.CommunityID)
                .Select(c => new
                {
                    CommunityName = c.Name,
                    SenderAddress = $"{c.Address}, {c.Municipality.Name}",
                    CommonSpaceName = commonSpace.Name,
                    Recipients = c.Users
                        .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            var existeMaintenance = await _context.Maintenances.Where(x => x.CommonSpaceID == commonSpace.ID && x.IsActive).FirstOrDefaultAsync(cancellationToken);

            if (request.IsInMaintenance && existeMaintenance != null && 
                existeMaintenance.StartDate == startDate && 
                existeMaintenance.EndDate == endDate && 
                existeMaintenance.Comment == request.Comment)
            {
                return Result.Success();
            }
            else if (request.IsInMaintenance && existeMaintenance != null)
            {
                    existeMaintenance.StartDate = startDate;
                    existeMaintenance.EndDate = endDate;
                    existeMaintenance.Comment = request.Comment;
            } else
            {
                var newMaintenance = new Models.Maintenance
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Comment = request.Comment,
                    CommonSpaceID = commonSpace.ID,
                    IsActive = true,
                    CommunityID = request.CommunityID,
                };
                _ = await _context.Maintenances.AddAsync(newMaintenance, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);


            var templateNotification = await _context.TemplateNotifications.Where(t => t.Name == "Maintenance").FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (templateNotification == null) return Result.Failure(NotificationErrors.TemplateNotFoundOnMaintenance);
            if (string.IsNullOrWhiteSpace(templateNotification.TemplateId)) return Result.Failure(NotificationErrors.TemplateIdIsNullOnMaintenance);

            var from = new EmailAddress("intelificio@duocuc.cl", $"{communityData.CommunityName} {" a través de Intelificio"}");

            var templates = new List<MaintenanceTemplate>();

            string[] diasSemana = ["domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado"];
            string[] meses = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];

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

            var result = await _sendMail.SendMaintenanceNotificationToMultipleRecipients(from, communityData.Recipients, templateNotification.TemplateId, templates);
            if (!result.IsSuccessStatusCode) return Result.Failure(NotificationErrors.EmailNotSentOnPackage);
            return Result.Success();

        }

    }
}
