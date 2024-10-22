using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Authentication.Commands.Signup
{
    public class SignUpCommand : IRequest<List<Result>>
    {
        public UserObject? User { get; set; }
        public List<UserObject>? Users { get; set; }
        public int CommunityID { get; set; }

        public bool IsMassive { get; set; } = false;
    }

}
