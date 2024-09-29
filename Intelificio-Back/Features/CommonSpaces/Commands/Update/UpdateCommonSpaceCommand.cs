using Backend.Common.Response;
using MediatR;

namespace Backend.Features.CommonSpaces.Commands.Update
{
    public class UpdateCommonSpaceCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool IsInMaintenance { get; set; }

    }
}
