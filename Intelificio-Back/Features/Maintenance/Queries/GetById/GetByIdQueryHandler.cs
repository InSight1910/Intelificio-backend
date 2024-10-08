using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Maintenance.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Maintenance.Queries.GetById
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIdQueryHandler> _logger;

        public GetByIdQueryHandler(IntelificioDbContext context, ILogger<GetByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByIdQuery request, CancellationToken cancellation)
        {
            var maintenance = await _context.Maintenances.FirstOrDefaultAsync(x => x.ID == request.MaintenanceId);
            if (maintenance is null) return Result.Failure(MaintenanceErrors.MaintenanceNotFoundOnQuery);

            var response = new GetByIdQueryResponse
            { 
                StartDate = maintenance.StartDate.ToString("dd-MM-yyyy"),
                EndDate = maintenance.EndDate.ToString("dd-MM-yyyy"),
                Comment = maintenance.Comment,
                CommonSpaceID = maintenance.CommonSpaceID,
                CommunityID = maintenance.CommunityID,
                CommonSpaceName = maintenance.CommonSpace.Name,
                CommonSpaceLocation = maintenance.CommonSpace.Location,
                IsActive = maintenance.IsActive
            };

            return Result.WithResponse(new ResponseData()
            {
                Data = response
            });

        }

    }
}
