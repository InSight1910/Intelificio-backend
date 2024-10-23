using AutoMapper;
using Backend.Common.Response;
using Backend.Features.AssignedFines.Common;
using Backend.Features.AssignedFines.Queries.GetAssignedFinesById;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeZoneConverter;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId
{
    public class GetAssignedFinesByUserIdQueryHandler(IntelificioDbContext context, ILogger<GetAssignedFinesByUserIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetAssignedFinesByUserIdQuery, Result>
    {
        private readonly IntelificioDbContext _context = context;
        private readonly ILogger<GetAssignedFinesByUserIdQueryHandler> _logger = logger;
        private IMapper _mapper = mapper;

        public async Task<Result> Handle(GetAssignedFinesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var checkUSer = await _context.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            if (!checkUSer) return Result.Failure(AssignedFinesErrors.UserNotFoundOnGetAssignedFinesByUserId);

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
                    UnitFloor = x.Unit.Floor,
                    UnitBuildingName = x.Unit.Building.Name,
                    EventDate = TimeZoneInfo.ConvertTimeFromUtc(x.EventDate, TZConvert.GetTimeZoneInfo(x.Fine.Community.TimeZone)).ToString("dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    Comment = x.Comment,
                    Fineamount = x.Fine.Amount,
                    FineName = x.Fine.Name,
                    FineStatus = x.Fine.Status,
                })
                .ToListAsync(cancellationToken);
            if (assignedFines is null) return Result.Failure(AssignedFinesErrors.AssignedFineNotFoundOnGetAssignedFinesByUserIdQuery);

            return Result.WithResponse(new ResponseData() { Data = assignedFines });
        }
    }
}
