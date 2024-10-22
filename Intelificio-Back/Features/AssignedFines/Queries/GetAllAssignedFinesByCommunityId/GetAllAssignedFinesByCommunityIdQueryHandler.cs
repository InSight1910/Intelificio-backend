using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Backend.Features.AssignedFines.Queries.GetAllAssignedFinesByCommunity
{
    public class GetAllAssignedFinesByCommunityIdQueryHandler(IntelificioDbContext context,ILogger<GetAllAssignedFinesByCommunityIdQueryHandler> logger, IMapper mapper)
        : IRequestHandler<GetAllAssignedFinesByCommunityIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAllAssignedFinesByCommunityIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAllAssignedFinesByCommunityIdQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId, cancellationToken);
            if (!checkCommunity) return Result.Failure(AssignedFinesErrors.CommunityNotFoundOnGetAllAssignedFinesByCommunity);

            var AssignedFines = await _context.AssignedFines
            .Include(x => x.Unit)
            .ThenInclude(x => x.Building)
            .ThenInclude(x => x.Community)
            .Where(x => x.Unit.Building.Community.ID == request.CommunityId)
            .ToListAsync(cancellationToken);

            var response = _mapper.Map<GetAllAssignedFinesByCommunityIdQueryResponse>(AssignedFines);

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });
        }

    }
}
