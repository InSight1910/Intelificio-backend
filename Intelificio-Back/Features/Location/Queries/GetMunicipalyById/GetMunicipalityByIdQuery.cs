using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Location.Queries.GetMunicipalyById
{
    public class GetMunicipalityByIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int MunicipalityId { get; set; }
    }
}
