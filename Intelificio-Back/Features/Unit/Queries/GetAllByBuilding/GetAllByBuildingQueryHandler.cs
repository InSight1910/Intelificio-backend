using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetAllByBuilding;

public class GetAllByBuildingQueryHandler : IRequestHandler<GetAllByBuildingQuery, Result>
{
    private readonly IntelificioDbContext _context;
    private readonly ILogger<GetAllByBuildingQueryHandler> _logger;

    public GetAllByBuildingQueryHandler(IntelificioDbContext context, ILogger<GetAllByBuildingQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(GetAllByBuildingQuery request, CancellationToken cancellationToken)
    {
        var units = await _context.Units
            .Include(x => x.Building)
            .Include(x => x.Users)
            //.Include(x => x.User)  // Incluye el usuario principal
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
                        .Select(user => $"{user.FirstName} {user.LastName}")
                        .FirstOrDefault() ?? "Sin Asignar"
            })
            .ToListAsync();

        return Result.WithResponse(new ResponseData
        {
            Data = units
        });
    }
}