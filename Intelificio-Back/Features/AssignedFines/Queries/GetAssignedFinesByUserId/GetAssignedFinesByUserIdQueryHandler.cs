using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId
{
    public class GetAssignedFinesByUserIdQueryHandler(IntelificioDbContext context,
        ILogger<GetAssignedFinesByUserIdQueryHandler> logger, 
        UserManager<User> manager) : IRequestHandler<GetAssignedFinesByUserIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByUserIdQueryHandler> _logger = logger;
        private readonly UserManager<User> _userManager = manager;

        public async Task<Result> Handle(GetAssignedFinesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var checkUSer = await _context.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            if (!checkUSer) return Result.Failure(AssignedFinesErrors.UserNotFoundOnGetAssignedFinesByUserId);

            var ownerIds = (await _userManager.GetUsersInRoleAsync("Propietario")).Select(o => o.Id).ToList();

            var assignedFines = await _context.AssignedFines
                .Include(x => x.Unit)
                .ThenInclude(x => x.Building)
                .Include(x => x.Fine)
                .ThenInclude(x => x.Community)
                .Where(x => x.Unit.Users.Any(u => u.Id == request.UserId))
                .Select(x => new GetAssignedFinesByUserIdQueryResponse
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
                })
                .ToListAsync(cancellationToken);
            if (assignedFines is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedFinesByUserIdQuery);

            return Result.WithResponse(new ResponseData() { Data = assignedFines });
        }
    }
}
