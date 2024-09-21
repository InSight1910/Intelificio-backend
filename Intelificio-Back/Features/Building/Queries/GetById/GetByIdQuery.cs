using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Building.Queries.GetById
{
    public class GetByIDQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int BuildingId { get; set; }
    }
}
