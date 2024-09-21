using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Queries.GetByID
{
    public class GetByIDQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int? UnitId { get; set; }
    }
}
