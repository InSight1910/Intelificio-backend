using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAll
{
    public class GetAllCommunitiesQueryHandler : IRequestHandler<GetAllCommunitiesQuery, Result>
    {
        private readonly ILogger<GetAllCommunitiesQueryHandler> _logger;
        private readonly IntelificioDbContext _context;

        public GetAllCommunitiesQueryHandler(ILogger<GetAllCommunitiesQueryHandler> logger, IntelificioDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            var adminRoleId = _context.Roles.FirstOrDefault(x => x.Name == "Administrador")?.Id;
            var communities = await _context.Community
                .Include(x => x.Municipality.City.Region)
                .IgnoreQueryFilters()
                .Select(x => new GetAllCommunitiesResponse
                {
                    Id = x.ID,
                    Address = x.Address,
                    CreationDate = x.FoundationDate,
                    Name = x.Name,
                    Municipality = x.Municipality.Name,
                    MunicipalityId = x.Municipality.ID,
                    City = x.Municipality.City.Name,
                    CityId = x.Municipality.City.ID,
                    Region = x.Municipality.City.Region.Name,
                    RegionId = x.Municipality.City.Region.ID,
                    AdminName = x.Users
                                    .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                                    .Select(u => u.ToString())
                                    .FirstOrDefault() ?? "Sin Administrador",
                    AdminId = x.Users
                                    .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                                    .Select(u => u.Id)
                                    .FirstOrDefault()
                })
                .ToListAsync();
            return Result.WithResponse(new ResponseData
            {
                Data = communities
            });
        }
    }
}
