using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Buildings.Commands.RemoveUnit
{
    public class RemoveUnitBuildingCommand : IRequest<Result>
    {
        [JsonIgnore]
        public required int BuildingId { get; set; }
        [JsonIgnore]
        public required int UnitId { get; set; }
    }
}
