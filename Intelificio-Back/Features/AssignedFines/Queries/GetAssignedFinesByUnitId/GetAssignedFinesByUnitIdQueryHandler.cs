using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUnitId
{
    public class GetAssignedFinesByUnitIdQueryHandler(IntelificioDbContext context, ILogger<GetAssignedFinesByUnitIdQueryHandler> logger, IMapper mapper): IRequestHandler<GetAssignedFinesByUnitIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByUnitIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAssignedFinesByUnitIdQuery request, CancellationToken cancellationToken)
        {
            var assignedfine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.UnitId == request.UnitId, cancellationToken);
            if (assignedfine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedFinesByUnitIdQuery);

            var response = _mapper.Map<GetAssignedFinesByUnitIdQueryResponse>(assignedfine);
            return Result.WithResponse(new ResponseData() { Data = response });
        }
    }
}
