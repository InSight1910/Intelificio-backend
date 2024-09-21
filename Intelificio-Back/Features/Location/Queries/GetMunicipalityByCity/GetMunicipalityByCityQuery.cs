using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Location.Queries.GetMunicipalityByCity
{
    public class GetMunicipalityByCityQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int CityId { get; set; }
    }
}
