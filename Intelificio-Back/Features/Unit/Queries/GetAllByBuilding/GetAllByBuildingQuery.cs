using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Queries.GetAllByBuilding
{
    public class GetAllByBuildingQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int BuildingId { get; set; }
    }
}