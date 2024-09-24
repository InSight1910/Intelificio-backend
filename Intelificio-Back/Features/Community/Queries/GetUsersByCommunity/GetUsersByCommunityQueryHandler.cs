using Backend.Common.Response;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Community.Queries.GetUsersByCommunity
{
    public class GetUsersByCommunityQueryHandler : IRequestHandler<GetUsersByCommunityQuery, Result>
    {
        private readonly IntelificioDbContext _context;
        private readonly UserManager<User> _userManager;

        public GetUsersByCommunityQueryHandler(IntelificioDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetUsersByCommunityQuery request, CancellationToken cancellationToken)
        {
            var community = await _context.Community.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == request.CommunityId);
            if (community == null)
            {
                return Result.Failure(null);
            }

            var usersTasks = new List<GetUsersByCommunityQueryResponse>();

            foreach (var u in community.Users)
            {
                var unitCount = await _context.Units.CountAsync(x => x.ID == u.Id);
                var roles = await _userManager.GetRolesAsync(u);

                // Add the processed user response to the list
                usersTasks.Add(new GetUsersByCommunityQueryResponse
                {
                    Id = u.Id,
                    Name = string.Format("{0} {1}", u.FirstName, u.LastName),
                    Email = u.Email!,
                    PhoneNumber = u.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? "Sin Rol",
                    UnitCount = unitCount
                });
            }

            return Result.WithResponse(new ResponseData
            {
                Data = usersTasks
            });
        }
    }
}
