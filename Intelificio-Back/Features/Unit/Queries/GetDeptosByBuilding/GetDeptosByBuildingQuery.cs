using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Queries.GetDeptosByBuilding
{
    public class GetDeptosByBuildingQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int BuildingId { get; set; }
    }
}