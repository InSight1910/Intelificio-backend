using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId
{
    public class GetAssignedFinesByUserIdQueryHandler(IntelificioDbContext context, ILogger<GetAssignedFinesByUserIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAssignedFinesByUserIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByUserIdQueryHandler> _logger = logger;
        private IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAssignedFinesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var assignedFine = await _context.AssignedFines
                .Include(x => x.Unit)
                .ThenInclude(x => x.Users)
                .FirstOrDefaultAsync(x => x.Unit.UserId == request.UserId, cancellationToken);
            if (assignedFine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedFinesByUserIdQuery);

            var response = _mapper.Map<GetAssignedFinesByUserIdQueryResponse>(assignedFine);
            return Result.WithResponse(new ResponseData() { Data = response });
        }
    }
}
