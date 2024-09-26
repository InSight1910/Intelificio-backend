using Backend.Common.Response;
using Backend.Features.Authentication.Common;
using Backend.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features.Authentication.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result>
    {
        private readonly UserManager<User> _userManager;

        public GetUserByEmailQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user is null) return Result.Failure(AuthenticationErrors.UserNotFoundGetByEmail);
            var role = await _userManager.GetRolesAsync(user);
            var response = new GetUserByEmailQueryResponse
            {
                Id = user.Id,
                Name = string.Format("{0} {1}", user.FirstName, user.LastName),
                PhoneNumber = user.PhoneNumber,
                Role = role.FirstOrDefault("Sin Rol"),
            };



            return Result.WithResponse(new ResponseData
            {
                Data = response
            });
        }
    }
}
