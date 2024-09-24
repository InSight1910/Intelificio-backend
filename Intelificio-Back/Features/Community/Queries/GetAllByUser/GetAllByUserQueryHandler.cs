using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAllByUser
{
    public class GetAllByUserQueryHandler : IRequestHandler<GetAllByUserQuery, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByUserQueryHandler> _logger;

        public GetAllByUserQueryHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IntelificioDbContext context, ILogger<GetAllByUserQueryHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByUserQuery request, CancellationToken cancellationToken)
        {
            var checkUser = await _userManager.Users.AnyAsync(x => x.Id == request.UserId);

            if (!checkUser) return Result.Failure(CommunityErrors.UserNotFound);
            try
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync("Administrador");
                var adminUserIds = adminUsers.Select(u => u.Id).ToList();
                var adminUsersWithCommunities = await _context.Users
                                                    .Where(u => adminUserIds.Contains(u.Id))
                                                    .Include(u => u.Communities)             // Include their communities
                                                    .ToListAsync();

                var communities = _context.Community
                                                .Where(x => x.Users.Any(u => u.Id == request.UserId))
                                                .AsEnumerable()
                                                .Select(x => new GetAllByUserResponse
                                                {
                                                    Id = x.ID,
                                                    Name = x.Name,
                                                    Address = x.Address,
                                                    BuildingCount = x.Buildings.Count(),
                                                    UnitCount = x.Buildings.SelectMany(b => b.Units).Count(),
                                                    AdminName = adminUsersWithCommunities
                                                                    .Where(u => u.Communities.Any(c => c.ID == x.ID))
                                                                    .Select(u => $"{u.FirstName} {u.LastName}")
                                                                    .FirstOrDefault() ?? "Sin Administrador"
                                                })
                                                .ToList();
                return Result.WithResponse(new ResponseData()
                {
                    Data = communities
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las comunidades del usuario");
                return Result.Failure(null);
            }



        }
    }
}
