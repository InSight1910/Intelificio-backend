using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Queries.GetById
{
    public class GetByIdUnitQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int? UnitId { get; set; }
    }
}
