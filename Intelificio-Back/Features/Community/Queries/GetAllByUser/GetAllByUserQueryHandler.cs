using Backend.Common.Response;
using Backend.Features.Community.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetAllByUser;

public class GetAllByUserQueryHandler : IRequestHandler<GetAllByUserQuery, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IntelificioDbContext _context;
    private readonly ILogger<GetAllByUserQueryHandler> _logger;

    public GetAllByUserQueryHandler(UserManager<User> userManager, RoleManager<Role> roleManager,
        IntelificioDbContext context, ILogger<GetAllByUserQueryHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(GetAllByUserQuery request, CancellationToken cancellationToken)
    {
        var checkUser = await _context.Users.AnyAsync(x => x.Id == request.UserId);

        if (!checkUser) return Result.Failure(CommunityErrors.UserNotFound);
        try
        {
            var adminRoleId = await _context.Roles
                .Where(r => r.Name == "Administrador")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
            var communities = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Include(x => x.Communities)
                .ThenInclude(x => x.Buildings)
                .ThenInclude(x => x.Units)
                .SelectMany(x => x.Communities)
                .Select(x => new GetAllByUserResponse
                {
                    Id = x.ID,
                    Name = x.Name,
                    Address = x.Address,
                    BuildingCount = x.Buildings.Count,
                    UnitCount = x.Buildings.SelectMany(x => x.Units).Count(),
                    AdminName = x.Users
                        .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                        .Select(u => u.ToString())
                        .FirstOrDefault() ?? "Sin Administrador"
                }).ToListAsync();


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