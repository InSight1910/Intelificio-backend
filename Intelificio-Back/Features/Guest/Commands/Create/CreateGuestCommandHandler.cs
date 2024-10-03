using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var checkUnit = await _context.Units.FirstOrDefaultAsync(x => x.ID == request.UnitId);
            if (checkUnit == null) return Result.Failure(GuestErrors.UnitNotFoundCreateGuest);

            var checkGuestId = await _context.Guest.AnyAsync(x => x.ID == request.Id && x.Unit == checkUnit);
            if (checkGuestId) return Result.Failure(GuestErrors.GuestAlreadyExists);

            var newGuest = _mapper.Map<Models.Guest>(request);

            newGuest.Unit = checkUnit;

            _ = await _context.Guest.AddAsync(newGuest);
            _ = await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
