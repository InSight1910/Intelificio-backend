using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Buildings.Commands.Update
{
    public class UpdateBuildingCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int CommunityId { get; set; }
        public required int Floors { get; set; }

    }
}
