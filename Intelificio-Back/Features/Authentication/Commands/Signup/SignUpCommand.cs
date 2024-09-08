using Intelificio_Back.Common.Response;
using Intelificio_Back.Models;
using MediatR;

namespace Intelificio_Back.Features.Authentication.Commands.Signup
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
        public DateTime Admission { get; set; }
        public required Role Role { get; set; }
    }
}
