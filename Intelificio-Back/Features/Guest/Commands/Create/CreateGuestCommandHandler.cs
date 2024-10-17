using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeZoneConverter;

namespace Backend.Features.Guest.Commands.Create
{
    public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<CreateGuestCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateGuestCommandHandler(IntelificioDbContext context, ILogger<CreateGuestCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {

            
            var unit = await _context.Units.Where(x => x.ID == request.UnitId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (unit is null) return Result.Failure(GuestErrors.UnitNotFoundCreateGuest);

            var community = await _context.Community.Where(c => c.ID == request.CommunityId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (community is null) return Result.Failure(GuestErrors.CommunityNotFoundOnCreateGuest);


            var newGuest = _mapper.Map<Models.Guest>(request);
            newGuest.EntryTime = TimeZoneInfo.ConvertTimeFromUtc((DateTime)request.EntryTime, TZConvert.GetTimeZoneInfo(community.TimeZone));

            _ = await _context.Guest.AddAsync(newGuest);
            _ = await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
