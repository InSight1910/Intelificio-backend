using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAllAssignedFinesByCommunity
{
    public class GetAllAssignedFinesByCommunityIdQueryHandler(IntelificioDbContext context,
        ILogger<GetAllAssignedFinesByCommunityIdQueryHandler> logger,
        UserManager<User> manager)
        : IRequestHandler<GetAllAssignedFinesByCommunityIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAllAssignedFinesByCommunityIdQueryHandler> _logger = logger;
        private readonly UserManager<User> _userManager = manager;

        public async Task<Result> Handle(GetAllAssignedFinesByCommunityIdQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId, cancellationToken);
            if (!checkCommunity) return Result.Failure(AssignedFinesErrors.CommunityNotFoundOnGetAllAssignedFinesByCommunity);

            var ownerIds = (await _userManager.GetUsersInRoleAsync("Propietario")).Select(o => o.Id).ToList();

            var AssignedFines = await _context.AssignedFines
                .Include(x => x.Unit)
                .ThenInclude(x => x.Building)
                .Include(x => x.Fine)
                .ThenInclude(x => x.Community)
                .Where(x => x.Unit.Building.Community.ID == request.CommunityId)
                .Select(x => new GetAllAssignedFinesByCommunityIdQueryResponse
                {
                    AssignedFineID = x.ID,
                    FineId = x.FineId,
                    UnitId = x.UnitId,
                    UnitNumber = x.Unit.Number,
                    UnitType = x.Unit.UnitType.Description,
                    UnitFloor = x.Unit.Floor,
                    UnitBuildingName = x.Unit.Building.Name,
                    UnitBuildingId = x.Unit.BuildingId,
                    EventDate = TimeZoneInfo.ConvertTimeFromUtc(x.EventDate, TZConvert.GetTimeZoneInfo(x.Fine.Community.TimeZone)).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    Comment = x.Comment,
                    FineAmount = x.Fine.Amount,
                    FineName = x.Fine.Name,
                    FineStatus = x.Fine.Status,
                    User = x.Unit.Users
                        .Where(user => ownerIds.Contains(user.Id))  
                        .Select(user => user.ToString())
                        .FirstOrDefault() ?? "Sin Asignar"
                }).ToListAsync(cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = AssignedFines
            });
        }

    }
}
