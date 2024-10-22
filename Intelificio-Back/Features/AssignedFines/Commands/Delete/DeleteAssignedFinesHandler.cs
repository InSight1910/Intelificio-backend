using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.AssignedFines.Commands.Delete
{
    public class DeleteAssignedFinesHandler(IntelificioDbContext context, ILogger<DeleteAssignedFinesHandler> logger): IRequestHandler<DeleteAssignedFinesCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<DeleteAssignedFinesHandler> _logger = logger;
        
        public async Task<Result> Handle(DeleteAssignedFinesCommand request, CancellationToken cancellationToken)
        {
            var assignedFine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.ID == request.AssignedfineId, cancellationToken);
            if (assignedFine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnDeleteAssignedFines);

            _ = _context.AssignedFines.Remove(assignedFine);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }

    }
}
