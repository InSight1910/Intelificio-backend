using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Queries.GetAllByEntryTime
{
    public class GetAllByEntryTimeGuestQueryHandler : IRequestHandler<GetAllByEntryTimeGuestQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByEntryTimeGuestQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllByEntryTimeGuestQueryHandler(IntelificioDbContext context, ILogger<GetAllByEntryTimeGuestQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByEntryTimeGuestQuery request, CancellationToken cancellationToken)
        {
            var guests = await _context.Guest
                .Where(x => x.EntryTime.Date == request.EntryTime.Date)
                .Select(x => new GetAllByEntryTimeGuestQueryResponse
                {
                    Id = x.ID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Rut = x.Rut,
                    EntryTime = x.EntryTime,
                    Plate = x.Plate,
                    UnitId = x.Unit.ID
                })
                .ToListAsync();

            if (guests == null) return Result.Failure(GuestErrors.GuestNotFoundGetById);

            return Result.WithResponse(new ResponseData
            {
                Data = guests
            });
        }
    }
}
