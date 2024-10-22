using Backend.Common.Response;
using Backend.Features.Fine.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Fine.Commands.Delete
{
    public class DeleteFineHandler(IntelificioDbContext conext, ILogger<DeleteFineHandler> logger) : IRequestHandler<DeleteFineCommand, Result>
    {
        private readonly IntelificioDbContext _context = conext;
        private readonly ILogger<DeleteFineHandler> _logger = logger;

        public async Task<Result> Handle(DeleteFineCommand request, CancellationToken cancellationToken)
        {
            var fine = await _context.Fine.FirstOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
            if (fine is null) return Result.Failure(FineErrors.FineNotFoundOnDeleteFine);

            var assignedfine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.Fine.ID == fine.ID, cancellationToken);
            if (assignedfine is not null) return Result.Failure(FineErrors.FineIsAssociatedToOneOrMoreAsignedFinesOnDeleteFine);

            _ = _context.Fine.Remove(fine);
            _ = await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }
    }
}
