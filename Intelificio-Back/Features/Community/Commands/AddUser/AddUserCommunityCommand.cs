using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Commands.AddUser
{
    public class AddUserCommunityCommand : IRequest<Result>
    {

        public AddUserObject? User { get; set; }
        public List<AddUserObject>? Users { get; set; }
    }
}