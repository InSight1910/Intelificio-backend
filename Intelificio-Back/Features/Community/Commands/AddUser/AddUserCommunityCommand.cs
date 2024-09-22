using Backend.Common.Response;
using Backend.Features.Community.Commands.AddUser;
using MediatR;

namespace Backend.Features.Community.Commands.Assign
{
    public class AddUserCommunityCommand : IRequest<Result>
    {

        public AddUserObject? User { get; set; }
        public List<AddUserObject>? Users { get; set; }
    }
}