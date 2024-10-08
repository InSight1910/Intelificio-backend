using Backend.Common.Response;
using Backend.Features.CommonSpaces.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.CommonSpaces.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryHandler : IRequestHandler<GetAllByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetAllByCommunityQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetAllByCommunityQuery request, CancellationToken cancellationToken)
        {
            var existCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);
            if (!existCommunity) return Result.Failure(CommonSpacesErrors.CommunityNotFoundOnQuery);

            var commonSpaces = await _context.CommonSpaces
                .Include(x => x.Maintenances) 
                .Where(x => x.CommunityId == request.CommunityId) 
                .Select(x => new GetAllByCommunityQueryResponse
                {
                    ID = x.ID,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Location = x.Location,
                    IsInMaintenance = x.IsInMaintenance,
                    
                    StartDate = x.Maintenances
                        .Where(m => m.IsActive) 
                        .Select(m => m.StartDate.ToString("yyyy-MM-dd")) 
                        .FirstOrDefault() ?? string.Empty, 

                    EndDate = x.Maintenances
                        .Where(m => m.IsActive)
                        .Select(m => m.EndDate.ToString("yyyy-MM-dd"))
                        .FirstOrDefault() ?? string.Empty,

                    Comment = x.Maintenances
                        .Where(m => m.IsActive)
                        .Select(m => m.Comment)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToListAsync(cancellationToken);

            return Result.WithResponse(new ResponseData { Data = commonSpaces });
        }
    }
}
