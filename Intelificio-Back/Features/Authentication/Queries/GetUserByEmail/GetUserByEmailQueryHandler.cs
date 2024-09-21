using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Authentication.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result>
    {
        private readonly IntelificioDbContext _context;

        public GetUserByEmailQueryHandler(IntelificioDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(x => x.Role).Where(x => x.Email == request.Email).Select(x => new GetUserByEmailQueryResponse
            {
                Id = x.Id,
                Name = string.Format("{0} {1}", x.FirstName, x.LastName),
                PhoneNumber = x.PhoneNumber,
                Role = x.Role.Name!,
            }).FirstOrDefaultAsync();

            if (user is null) return Result.Failure(AuthenticationErrors.UserNotFoundGetByEmail);

            return Result.WithResponse(new ResponseData
            {
                Data = user
            });
        }
    }
}
