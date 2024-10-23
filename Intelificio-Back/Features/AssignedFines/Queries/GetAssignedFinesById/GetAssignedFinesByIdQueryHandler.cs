using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQueryHandler(IntelificioDbContext context) : IRequestHandler<GetAssignedFinesByIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
    
        public async Task<Result> Handle(GetAssignedFinesByIdQuery request, CancellationToken cancellationToken) 
        {
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
                    UnitFloor = x.Unit.Floor,
                    UnitBuildingName = x.Unit.Building.Name,
                    EventDate = TimeZoneInfo.ConvertTimeFromUtc(x.EventDate, TZConvert.GetTimeZoneInfo(x.Fine.Community.TimeZone)).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    Comment = x.Comment,
                    Fineamount = x.Fine.Amount,
                    FineName = x.Fine.Name,
                    FineStatus = x.Fine.Status,
                }).FirstOrDefaultAsync(cancellationToken);

            if (assignedFine is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedfinesByIdQuery);

            return Result.WithResponse(new ResponseData() { Data = assignedFine });
        }
    }
}
