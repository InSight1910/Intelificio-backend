﻿using Backend.Common.Response;
using Backend.Features.Unit.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Unit.Queries.GetAllByBuilding
{
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
                .Where(x => x.Building.ID == request.BuildingId)
                .Select(x => new GetAllByBuildingQueryResponse
                {
                    UnitType = x.UnitType,
                    Number = x.Number,
                    Building = x.Building,
                    Floor = x.Floor,
                    Surface = x.Surface
                })
                .ToListAsync();

            if (units == null) return Result.Failure(UnitErrors.UnitNotFoundGetAllByBuilding);

            return Result.WithResponse(new ResponseData
            {
                Data = units
            });
        }
    }
}