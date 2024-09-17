using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Commands.Delete
{
    public class DeleteCommunityCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
