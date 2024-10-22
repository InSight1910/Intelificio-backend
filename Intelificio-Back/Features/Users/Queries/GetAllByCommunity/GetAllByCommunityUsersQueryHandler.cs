using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Users.Queries.GetAllByCommunity
{
    public class GetAllByCommunityUsersQueryHandler : IRequestHandler<GetAllByCommunityUsersQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly ILogger<GetAllByCommunityUsersQueryHandler> _logger;

        public GetAllByCommunityUsersQueryHandler(IntelificioDbContext context, ILogger<GetAllByCommunityUsersQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> Handle(GetAllByCommunityUsersQuery request, CancellationToken cancellationToken)
        {
            var checkCommunity = await _context.Community.AnyAsync(x => x.ID == request.CommunityId);

            if (!checkCommunity) return Result.Failure(Common.UsersError.CommunityNotFoundOnQuery);
            var users = await _context.Users
                       .Where(u => u.Communities.Any(c => c.ID == request.CommunityId))
                       .Select(u => new GetAllByCommunityUsersQueryResponse

                       {
                           Id = u.Id,
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Rut = u.Rut,
                           CommunityId = request.CommunityId,

                       }).ToListAsync(cancellationToken: cancellationToken);


            return Result.WithResponse(new ResponseData()
            {
                Data = users
            });
        }
    }
}
