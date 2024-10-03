using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Queries.GetById
{
    public class GetByIdGuestQueryHandler : IRequestHandler<GetByIdGuestQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetByIdGuestQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetByIdGuestQueryHandler(IntelificioDbContext context, ILogger<GetByIdGuestQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetByIdGuestQuery request, CancellationToken cancellationToken)
        {
            var guests = await _context.Guest
                .Where(x => x.ID == request.GuestId)
                .Select(x => new GetByIdGuestQueryResponse
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Rut = x.Rut,
                    EntryTime = x.EntryTime,
                    Plate = x.Plate,
                    Unit = x.Unit.Number
                }).FirstOrDefaultAsync();

            if (guests == null) return Result.Failure(GuestErrors.GuestNotFoundGetById);

            return Result.WithResponse(new ResponseData()
            {
                Data = guests
            });
        }
    }
}
