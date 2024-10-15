using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Queries.GetAllByUnit
{
    public class GetAllByUnitGuestQueryHandler : IRequestHandler<GetAllByUnitGuestQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByUnitGuestQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllByUnitGuestQueryHandler(IntelificioDbContext context, ILogger<GetAllByUnitGuestQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<Result> Handle(GetAllByUnitGuestQuery request, CancellationToken cancellationToken)
        {
            var guests = await _context.Guest
                .Where(x => x.Unit.ID == request.UnitId)
                .Select(x => new GetAllByUnitGuestQueryResponse
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
