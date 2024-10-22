using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQueryHandler(IntelificioDbContext context, ILogger<GetAssignedFinesByIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAssignedFinesByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByIdQuery> _logger = (ILogger<GetAssignedFinesByIdQuery>)logger;
        private readonly IMapper _mapper = mapper;
        
        public async Task<Result> Handle(GetAssignedFinesByIdQuery request, CancellationToken cancellationToken) 
        {
            var assignedFine = await _context.AssignedFines.FirstOrDefaultAsync(x => x.ID == request.AssignedFineId, cancellationToken);
            if (assignedFine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedfinesByIdQuery);

            var response = _mapper.Map<GetAssignedFinesByIdQueryResponse>(assignedFine);
            return Result.WithResponse(new ResponseData() { Data = response });
        }
    }
}
