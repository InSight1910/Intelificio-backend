using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
