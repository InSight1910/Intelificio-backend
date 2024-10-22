using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.AssignedFines.Commands.Update
{
    public class UpdateAssignedFinesHandler(IntelificioDbContext context, ILogger<UpdateAssignedFinesHandler> logger, IMapper mapper) : IRequestHandler<UpdateAssignedFinesCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<UpdateAssignedFinesHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(UpdateAssignedFinesCommand request, CancellationToken cancellationToken)
        {
            
            var assignedFine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.ID == request.AssignedFineId, cancellationToken);
            if (assignedFine is null)
                return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnUpdateAssignedFines);


            if (assignedFine.FineId != request.FineId || assignedFine.UnitId != request.UnitId)
            {
     
                var fine = await _context.Fine.SingleOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
                if (fine is null) return Result.Failure(AssignedFinesErrors.FineNotFoundOnUpdateAssignedFines);

         
                if (assignedFine.UnitId != request.UnitId)
                {
                    var unit = await _context.Units.SingleOrDefaultAsync(x => x.ID == request.UnitId, cancellationToken);
                    if (unit is null) return Result.Failure(AssignedFinesErrors.UnitNotFoundOnUpdateAssignedFines);
                    assignedFine.Unit = unit;
                }

                assignedFine.Fine = fine; 
            }

            _mapper.Map(request, assignedFine);

            _context.AssignedFines.Update(assignedFine);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UpdateAssignedFinesResponse>(assignedFine);
            return Result.WithResponse(new ResponseData { Data = response });
        }

    }
}
