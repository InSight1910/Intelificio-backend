using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Location.Queries.GetRegionById
{
    public class GetRegionByIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int MunicipalityId { get; set; }
    }
}
