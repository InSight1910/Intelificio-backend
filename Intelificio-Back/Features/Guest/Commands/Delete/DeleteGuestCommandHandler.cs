using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Commands.Delete
{
    public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<DeleteGuestCommandHandler> _logger;

        public DeleteGuestCommandHandler(IntelificioDbContext context, ILogger<DeleteGuestCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = await _context.Guest.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (guest == null) return Result.Failure(GuestErrors.GuestNotFoundDelete);

            _ = _context.Guest.Remove(guest);
            _ = await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
