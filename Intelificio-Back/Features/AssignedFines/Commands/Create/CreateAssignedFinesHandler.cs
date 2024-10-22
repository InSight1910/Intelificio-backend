using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.Notification.Commands.FineNotification;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Features.AssignedFines.Commands.Create
{
    public class CreateAssignedFinesHandler(IntelificioDbContext context, ILogger<CreateAssignedFinesHandler> logger, IMapper mapper, IMediator mediator): IRequestHandler<CreateAssignedFinesCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<CreateAssignedFinesHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;

        public async Task<Result> Handle(CreateAssignedFinesCommand request, CancellationToken cancellationToken)
        {
            var fine = await _context.Fine.FirstOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
            if (fine is null) return Result.Failure(AssignedFinesErrors.FineNotExistOnCreateAssignedFines);

            var unit = await _context.Units.Include(x => x.Building).Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.UnitId, cancellationToken);
            if (unit is null) return Result.Failure(AssignedFinesErrors.UnitNotExistOnCreateAssignedFines);

            if(fine.CommunityId != unit.Building.Community.ID)
            {
                return Result.Failure(AssignedFinesErrors.FineOrUnitAreDifferentCommunityOnCreateAssignedFines);
            }

            if (unit.Users.IsNullOrEmpty()) return Result.Failure(AssignedFinesErrors.UnitHasNotUsersOnCreateAssignedFines);

            var assignedFine = _mapper.Map<AssignedFine>(request);
            assignedFine.Unit = unit;
            assignedFine.Fine = fine;

            var NewAssignedFine = await _context.AssignedFines.AddAsync(assignedFine, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);

            var AssignedFineNotification = new FineNotificationCommand { AssignedFineId = NewAssignedFine.Entity.ID};
            var AssignedFineNotificationResult = await _mediator.Send(AssignedFineNotification, cancellationToken);
            if (AssignedFineNotificationResult.IsFailure) return Result.Failure(AssignedFinesErrors.EmailAssignedFineNotSend);

            return Result.Success();

        }

    }
}
