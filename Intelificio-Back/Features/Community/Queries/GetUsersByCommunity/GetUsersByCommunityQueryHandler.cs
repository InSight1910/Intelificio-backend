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
            var users = await _context.Community
                .Where(c => c.ID == request.CommunityId)
                .SelectMany(c => c.Users)
                .Select(user => new GetUsersByCommunityQueryResponse
                {
                    Id = user.Id,
                    Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    Role = _context.Roles.Where(r => r.Id == _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).FirstOrDefault()).Select(r => r.Name).FirstOrDefault()!,
                    UnitCount = user.Units.Where(u => u.Building.Community.ID == request.CommunityId).Count()
                }).ToListAsync();

            return Result.WithResponse(new ResponseData
            {
                Data = users
            });
        }
    }
}
