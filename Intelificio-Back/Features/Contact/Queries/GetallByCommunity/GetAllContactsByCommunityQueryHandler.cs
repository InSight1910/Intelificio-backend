using Backend.Common.Response;
using Backend.Features.Buildings.Common;
using Backend.Features.Contact.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Contact.Queries.GetallByCommunity
{
    public class GetAllContactsByCommunityQueryHandler : IRequestHandler<GetAllContactsByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllContactsByCommunityQueryHandler> _logger;

        public GetAllContactsByCommunityQueryHandler(IntelificioDbContext context, ILogger<GetAllContactsByCommunityQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllContactsByCommunityQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);
            if (!checkCommunity) return Result.Failure(ContactErrors.CommunityNotFoundOnQuery);

            var contacts = await _context.Contacts
                .Where(c => c.Community.ID == request.CommunityId)
                .Select(c => new GetAllContactsByCommunityQueryResponse
                {
                    Id = c.ID,
                    Name = c.Name,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Service = c.Service,
                    CommunityId = c.Community.ID,
                }).ToListAsync(cancellationToken: cancellationToken);

            return Result.WithResponse(new ResponseData()
            {
                Data = contacts
            });
        }
    }
}
