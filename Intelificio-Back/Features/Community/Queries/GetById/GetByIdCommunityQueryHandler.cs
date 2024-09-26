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
            var adminRoleId = await _context.Roles
                    .Where(r => r.Name == "Administrador")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();


            var community = _context.Community
                                            .Include(x => x.Municipality)
                                            .ThenInclude(x => x.City)
                                            .ThenInclude(x => x.Region)
                                            .Where(x => x.ID == request.Id)
                                            .Select(x => new GetByIdCommunityResponse
                                            {
                                                Id = x.ID,
                                                Name = x.Name,
                                                Address = x.Address,
                                                MunicipalityId = x.Municipality.ID,
                                                CityId = x.Municipality.City.ID,
                                                RegionId = x.Municipality.City.Region.ID,
                                                AdminName = x.Users
                                                                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                                                                .Select(u => u.ToString())
                                                                .FirstOrDefault() ?? "Sin Administrador"
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
