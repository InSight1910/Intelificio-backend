using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUnitId
{
    public class GetAssignedFinesByUnitIdQueryHandler(IntelificioDbContext context, ILogger<GetAssignedFinesByUnitIdQueryHandler> logger, IMapper mapper): IRequestHandler<GetAssignedFinesByUnitIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByUnitIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAssignedFinesByUnitIdQuery request, CancellationToken cancellationToken)
        {

            var checkUnit = await _context.Units.AnyAsync(x => x.ID == request.UnitId, cancellationToken);
            if (!checkUnit) return Result.Failure(AssignedFinesErrors.UnitNotFoundOnGetAssignedfinesByUnitId);

            var assignedfines = await _context.AssignedFines
                .Include(x => x.Unit)
                .ThenInclude(x => x.Building)
                .Include(x => x.Fine)
                .ThenInclude(x => x.Community)
                .Where(x => x.UnitId == request.UnitId)
                .Select(x => new GetAssignedFinesByUnitIdQueryResponse
                    {
                    AssignedFineID = x.ID,
                    FineId = x.FineId,
                    UnitId = x.UnitId,
                    UnitNumber = x.Unit.Number,
                    UnitFloor = x.Unit.Floor,
                    UnitBuildingName = x.Unit.Building.Name,
                    EventDate = TimeZoneInfo.ConvertTimeFromUtc(x.EventDate, TZConvert.GetTimeZoneInfo(x.Fine.Community.TimeZone)).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    Comment = x.Comment,
                    FineAmount = x.Fine.Amount,
                    FineName = x.Fine.Name,
                    FineStatus = x.Fine.Status,
                }).ToListAsync(cancellationToken: cancellationToken);


            if (assignedfines.IsNullOrEmpty()) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedFinesByUnitIdQuery);

            
            return Result.WithResponse(new ResponseData() { Data = assignedfines });
        }
    }
}
