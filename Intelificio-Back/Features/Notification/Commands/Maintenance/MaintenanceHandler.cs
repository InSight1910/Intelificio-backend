//using AutoMapper;
//using Backend.Common.Helpers;
//using Backend.Common.Response;
//using Backend.Models;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using SendGrid.Helpers.Mail;

//namespace Backend.Features.Notification.Commands.Maintenance
//{
//    public class MaintenanceHandler : IRequest<MaintenanceCommand,Result>
//    {
//        private readonly SendMail _sendMail;
//        private readonly IntelificioDbContext _context;
//        private readonly ILogger<MaintenanceHandler> _logger;
//        private readonly IMapper _mapper;

//        public MaintenanceHandler(IntelificioDbContext context, ILogger<MaintenanceHandler> logger, IMapper mapper, SendMail sendMail)
//        {
//            _sendMail = sendMail;
//            _context = context;
//            _logger = logger;
//            _mapper = mapper;
//        }

//        public async Task<Result> Handle(MaintenanceCommand request, CancellationToken cancellationToken)
//        {
//            var communityData = new
//            {
//                CommunityName = string.Empty,
//                SenderAddress = string.Empty,
//                CommonSpaceName = string.Empty,
//                Recipients = new List<EmailAddress>()
//            };

//            if (request.Floor > 0)
//            {
//                communityData = await _context.Community
//                    .Include(c => c.Buildings)
//                    .ThenInclude(b => b.Units)
//                    .ThenInclude(u => u.Users)
//                    .Include(c => c.CommonSpaces)  // Usamos CommonSpaces en lugar de Spaces
//                    .Where(c => c.ID == request.CommunityID)
//                    .Select(c => new
//                    {
//                        CommunityName = c.Name ?? "",
//                        SenderAddress = c.Address,
//                        CommonSpaceName = c.CommonSpaces.FirstOrDefault(s => s.ID == request.CommonSpaceID)?.Name ?? "",  // Obtener el nombre del espacio común
//                        Recipients = c.Buildings
//                            .Where(b => b.ID == request.BuildingID)
//                            .SelectMany(b => b.Units)
//                            .Where(u => u.Floor == request.Floor)
//                            .SelectMany(u => u.Users)
//                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
//                            .ToList()
//                    })
//                    .FirstOrDefaultAsync();
//            }
//            // Caso 2: Filtrar por edificio
//            else if (request.BuildingID > 0)
//            {
//                communityData = await _context.Community
//                    .Include(c => c.Buildings)
//                    .ThenInclude(b => b.Units)
//                    .ThenInclude(u => u.Users)
//                    .Include(c => c.CommonSpaces)  // Usamos CommonSpaces en lugar de Spaces
//                    .Where(c => c.ID == request.CommunityID)
//                    .Select(c => new
//                    {
//                        CommunityName = c.Name ?? "",
//                        SenderAddress = c.Address,
//                        CommonSpaceName = c.CommonSpaces.FirstOrDefault(s => s.ID == request.CommonSpaceID)?.Name ?? "",  // Obtener el nombre del espacio común
//                        Recipients = c.Buildings
//                            .Where(b => b.ID == request.BuildingID)
//                            .SelectMany(b => b.Units)
//                            .SelectMany(u => u.Users)
//                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
//                            .ToList()
//                    })
//                    .FirstOrDefaultAsync();
//            }
//            // Caso 3: Filtrar por comunidad completa
//            else
//            {
//                communityData = await _context.Community
//                    .Include(c => c.Users)
//                    .Include(c => c.CommonSpaces)  // Usamos CommonSpaces en lugar de Spaces
//                    .Where(c => c.ID == request.CommunityID)
//                    .Select(c => new
//                    {
//                        CommunityName = c.Name ?? "",
//                        SenderAddress = c.Address,
//                        CommonSpaceName = c.CommonSpaces.FirstOrDefault(s => s.ID == request.CommonSpaceID)?.Name ?? "",  // Obtener el nombre del espacio común
//                        Recipients = c.Users
//                            .Select(user => new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}"))
//                            .ToList()
//                    })
//                    .FirstOrDefaultAsync();
//            }

//        }

//    }
//}
