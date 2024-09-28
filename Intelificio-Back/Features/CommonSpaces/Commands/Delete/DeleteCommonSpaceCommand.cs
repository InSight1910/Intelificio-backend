using Backend.Common.Response;
using MediatR;

namespace Backend.Features.CommonSpaces.Commands.Delete
{
    public class DeleteCommonSpaceCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
