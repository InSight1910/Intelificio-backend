using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommand : IRequest<Result>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required new string Email { get; set; }
        public required new string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Rut { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
