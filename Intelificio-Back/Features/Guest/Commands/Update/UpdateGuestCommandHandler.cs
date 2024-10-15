using AutoMapper;
using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Commands.Update
{
    public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<UpdateGuestCommandHandler> _logger;

        private readonly IMapper _mapper;

        public UpdateGuestCommandHandler(IntelificioDbContext context, ILogger<UpdateGuestCommandHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = await _context.Guest.FirstOrDefaultAsync(x => x.ID == request.Id);
            if (guest is null) return Result.Failure(GuestErrors.GuestNotFoundUpdate);

            guest = _mapper.Map(request, guest);
            _ = _context.Guest.Update(guest);
            _ = await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
