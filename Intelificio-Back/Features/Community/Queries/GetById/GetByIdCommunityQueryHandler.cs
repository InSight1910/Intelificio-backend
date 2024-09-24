using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetById
{
    public class GetByIdCommunityQueryHandler : IRequestHandler<GetByIdCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GetByIdCommunityQueryHandler> _logger;

        public GetByIdCommunityQueryHandler(IntelificioDbContext context, UserManager<User> userManager, ILogger<GetByIdCommunityQueryHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(GetByIdCommunityQuery request, CancellationToken cancellationToken)
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync("Administrador");
            var adminUserIds = adminUsers.Select(u => u.Id).ToList();
            var adminUsersWithCommunities = await _context.Users
                                                .Include(u => u.Communities)             // Include their communities
                                                .Where(u => adminUserIds.Contains(u.Id) && u.Communities.Any(x => x.ID == request.Id))
                                                .ToListAsync();


            var community = _context.Community
                                            .Include(x => x.Municipality)
                                            .ThenInclude(x => x.City)
                                            .ThenInclude(x => x.Region)
                                            .Where(x => x.ID == request.Id)
                                            .AsEnumerable()
                                            .Select(x => new GetByIdCommunityResponse
                                            {
                                                Id = x.ID,
                                                Name = x.Name,
                                                Address = x.Address,
                                                MunicipalityId = x.Municipality.ID,
                                                CityId = x.Municipality.City.ID,
                                                RegionId = x.Municipality.City.Region.ID,
                                                AdminName = adminUsersWithCommunities.Count > 0 ? adminUsersWithCommunities
                                                                .Where(u => u.Communities.Any(c => c.ID == x.ID))
                                                                .Select(u => $"{u.FirstName} {u.LastName}")
                                                                .FirstOrDefault() ?? "Sin Administrador" : "Sin Administrador"
                                            })
                                            .FirstOrDefault();





            if (community is null) return Result.Failure(CommunityErrors.CommunityNotFoundGetByID);

            return Result.WithResponse(new ResponseData
            {
                Data = community
            });
        }
    }
}
