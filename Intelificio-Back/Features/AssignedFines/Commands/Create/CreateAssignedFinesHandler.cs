using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.Notification.Commands.FineNotification;
using Backend.Features.Notification.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Cms;
using SendGrid.Helpers.Mail;

namespace Backend.Features.AssignedFines.Commands.Create
{
    public class CreateAssignedFinesHandler(IntelificioDbContext context, ILogger<CreateAssignedFinesHandler> logger, IMapper mapper, IMediator mediator, UserManager<User> manager) : IRequestHandler<CreateAssignedFinesCommand, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<CreateAssignedFinesHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly UserManager<User> _userManager = manager;

        public async Task<Result> Handle(CreateAssignedFinesCommand request, CancellationToken cancellationToken)
        {
            var fine = await _context.Fine
                .FirstOrDefaultAsync(x => x.ID == request.FineId, cancellationToken);
            if (fine is null) return Result.Failure(AssignedFinesErrors.FineNotExistOnCreateAssignedFines);

            var unit = await _context.Units
                .Include(x => x.Building)
                .ThenInclude(x => x.Community)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.ID == request.UnitId, cancellationToken);
            if (unit is null) return Result.Failure(AssignedFinesErrors.UnitNotExistOnCreateAssignedFines);

            if(fine.CommunityId != unit.Building.Community.ID)
            {
                return Result.Failure(AssignedFinesErrors.FineOrUnitAreDifferentCommunityOnCreateAssignedFines);
            }

            if (unit.Users.IsNullOrEmpty()) return Result.Failure(AssignedFinesErrors.UnitHasNotUsersOnCreateAssignedFines);

            var usersInUnit = unit.Users.ToList();
            var owners = await _userManager.GetUsersInRoleAsync("Propietario");
            var unitOwners = usersInUnit.Where(u => owners.Any(o => o.Id == u.Id)).ToList();
            if (unitOwners.IsNullOrEmpty()) return Result.Failure(AssignedFinesErrors.NoOwnersFoundOnCreateAssignedFines);
            


            var assignedFine = _mapper.Map<AssignedFine>(request); 

            var NewAssignedFine = await _context.AssignedFines.AddAsync(assignedFine, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);

            var AssignedFineNotification = new FineNotificationCommand { AssignedFineId = NewAssignedFine.Entity.ID};
            var AssignedFineNotificationResult = await _mediator.Send(AssignedFineNotification, cancellationToken);
            if (AssignedFineNotificationResult.IsFailure) return Result.Failure(AssignedFinesErrors.EmailAssignedFineNotSend);

            return Result.Success();

        }

    }
}
