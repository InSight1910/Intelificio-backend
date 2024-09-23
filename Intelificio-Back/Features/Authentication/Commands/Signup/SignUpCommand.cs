using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommand : IRequest<Result>
    {
        public UserObject? User { get; set; }
        public List<UserObject>? Users { get; set; }
    }




}
