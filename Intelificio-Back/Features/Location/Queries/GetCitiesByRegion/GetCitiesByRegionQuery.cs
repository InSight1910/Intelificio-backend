using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Location.Queries.GetCitiesByRegion
{
    public class GetCitiesByRegionQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int RegionId { get; set; }
    }
}
