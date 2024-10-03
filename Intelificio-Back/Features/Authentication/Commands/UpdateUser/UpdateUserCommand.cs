using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
