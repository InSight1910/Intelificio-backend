using Backend.Common.Helpers;
using Backend.Common.Response;
using Backend.Features.Notification.Commands.Package;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Notification.Commands.PackageDelivered
{
    public class PackageDeliveredHandler : IRequestHandler<PackageDeliveredCommand,Result>
    {
        private readonly SendMail _sendMail;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<PackageDeliveredHandler> _logger;

        public PackageDeliveredHandler(IntelificioDbContext context, ILogger<PackageDeliveredHandler> logger, SendMail sendMail)
        {
            _sendMail = sendMail;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(PackageDeliveredCommand request, CancellationToken cancellationToken)
        {
            var package = await _context.Package
            .Include(u => u.Recipient)
            .ThenInclude(c => c.Communities)
            .ThenInclude(m => m.Municipality)
            .Where(p => p.ID == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
            if (package == null) return Result.Failure(NotificationErrors.PackageNotFound);

            return Result.Success();
        }

    }
}
