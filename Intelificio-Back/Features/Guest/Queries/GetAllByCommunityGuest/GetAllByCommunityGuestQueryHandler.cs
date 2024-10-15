using Backend.Common.Response;
using Backend.Features.Guest.Common;
using Backend.Features.Guest.Queries.GetAllByCommunity;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Guest.Queries.GetAllByCommunityGuest
{
    public class GetAllByCommunityGuestQueryHandler : IRequestHandler<GetAllByCommunityGuestQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByCommunityGuestQueryHandler> _logger;

        public GetAllByCommunityGuestQueryHandler(IntelificioDbContext context, ILogger<GetAllByCommunityGuestQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByCommunityGuestQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);

            if (!checkCommunity) return Result.Failure(GuestErrors.CommunityNotFoundGetAllByCommunity);
          
            var guests = await _context.Guest
                .Where(x => x.Unit.Building.Community.ID == request.CommunityId)
                .Select(x => new GetAllByCommunityGuestQueryResponse
                {
                    Id = x.ID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Rut = x.Rut,
                    EntryTime = x.EntryTime,
                    Plate = x.Plate,
                    Unit = x.Unit.Number
               
                }).ToListAsync(cancellationToken: cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = guests
            });
        }
    }
}
