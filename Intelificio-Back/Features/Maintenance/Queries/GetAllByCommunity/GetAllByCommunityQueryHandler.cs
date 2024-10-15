using Backend.Common.Response;
using MediatR;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Features.Maintenance.Common;


namespace Backend.Features.Maintenance.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryHandler : IRequestHandler<GetAllByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByCommunityQueryHandler> _logger;

        public GetAllByCommunityQueryHandler(IntelificioDbContext context, ILogger<GetAllByCommunityQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByCommunityQuery request, CancellationToken cancellationToken)
        {

            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);
            if (!checkCommunity) return Result.Failure(MaintenanceErrors.CommunityNotFoundOnQuery);

            var maintenances = await _context.Maintenances
                .Where(c => c.CommunityID == request.CommunityId)
                .Select(c => new GetAllByCommunityResponse
                {
                    MaintenanceID = c.ID,
                    StartDate = c.StartDate.ToString("dd-MM-yyyy"),
                    EndDate = c.EndDate.ToString("dd-MM-yyyy"),
                    Comment = c.Comment,
                    CommonSpaceID = c.CommonSpaceID,
                    CommonSpaceName = c.CommonSpace.Name,
                    CommonSpaceLocation = c.CommonSpace.Location,
                    IsActive = c.IsActive
                }).ToListAsync(cancellationToken: cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = maintenances
            });

        }
    }
}
