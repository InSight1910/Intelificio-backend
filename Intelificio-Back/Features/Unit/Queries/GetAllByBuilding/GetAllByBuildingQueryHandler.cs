using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetAllByBuilding;

public class GetAllByBuildingQueryHandler(IntelificioDbContext context, ILogger<GetAllByBuildingQueryHandler> logger, UserManager<User> manager) : IRequestHandler<GetAllByBuildingQuery, Result>
{
    private readonly IntelificioDbContext _context = context;
    private readonly ILogger<GetAllByBuildingQueryHandler> _logger = logger;
    private readonly UserManager<User> _userManager = manager;

    public async Task<Result> Handle(GetAllByBuildingQuery request, CancellationToken cancellationToken)
    {

        var ownerIds = (await _userManager.GetUsersInRoleAsync("Propietario")).Select(o => o.Id).ToList();

        var units = await _context.Units
            .Include(x => x.Building)
            .Include(x => x.Users)
            .Where(x => x.Building.ID == request.BuildingId)
            .Select(x => new GetAllByBuildingQueryResponse
            {
                Id = x.ID,
                Number = x.Number,
                UnitType = x.UnitType.Description,
                Building = x.Building.Name,
                Floor = x.Floor,
                Surface = x.Surface,
                User = x.Users
                .Where(user => ownerIds.Contains(user.Id))  // Filtrar propietarios en la base de datos
                .Select(user => user.ToString())
                .FirstOrDefault() ?? "Sin Asignar"
            })
            .ToListAsync();

        return Result.WithResponse(new ResponseData
        {
            Data = units
        });
    }
}