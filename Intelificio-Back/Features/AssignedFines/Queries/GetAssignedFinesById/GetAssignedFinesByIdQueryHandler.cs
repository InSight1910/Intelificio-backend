using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQueryHandler(IntelificioDbContext context,
        UserManager<User> manager) : IRequestHandler<GetAssignedFinesByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly UserManager<User> _userManager = manager;

        public async Task<Result> Handle(GetAssignedFinesByIdQuery request, CancellationToken cancellationToken)
        {

            var ownerIds = (await _userManager.GetUsersInRoleAsync("Propietario")).Select(o => o.Id).ToList();

            var assignedFine = await _context.AssignedFines
                .Include(x => x.Unit)
                .ThenInclude(x => x.Building)
                .Include(x => x.Fine)
                .ThenInclude(x => x.Community)
                .Where(x => x.ID == request.AssignedFineId)
                .Select(x => new GetAssignedFinesByIdQueryResponse
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
                }).FirstOrDefaultAsync(cancellationToken);

            if (assignedFine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedfinesByIdQuery);

            return Result.WithResponse(new ResponseData() { Data = assignedFine });
        }
    }
}
