using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Building.Commands.Create
{
    public class CreateBuildingCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        public required int Floors { get; set; }
        public int CommunityId { get; set; }

    }
}
