using Backend.Common.Response;
using MediatR;

namespace Backend.Features.CommonSpaces.Commands.Create
{
    public class CreateCommonSpaceCommand : IRequest<Result>
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsInMaintenance { get; set; }
        public int CommunityId { get; set; }
    }
}
