using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Location.Queries.GetCityById
{
    public class GetCityByIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int CityId { get; set; }
    }
}
