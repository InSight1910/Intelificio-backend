using Backend.Common.Response;
using Backend.Features.CommonSpaces.Common;
using Backend.Models;
using Backend.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.CommonSpaces.Commands.Delete
{
    public class DeleteCommonSpaceCommandHandler : IRequestHandler<DeleteCommonSpaceCommand, Result>
    {
        private readonly IntelificioDbContext _context;

        public DeleteCommonSpaceCommandHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCommonSpaceCommand request, CancellationToken cancellationToken)
        {
            var space = await _context.CommonSpaces.Where(x => x.ID == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (space == null) return Result.Failure(CommonSpacesErrors.CommonSpaceNotFoundOnDelete);
            if (space.Reservations.Any(x => x.Status == ReservationStatus.CONFIRMED || x.Status == ReservationStatus.PENDING)) return Result.Failure(CommonSpacesErrors.HasPendingReservationsOnDelete);

            _context.CommonSpaces.Remove(space);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
