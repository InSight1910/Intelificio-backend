using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Queries.GetByUser
{
    public class GetByUserQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int? UserId { get; set; }
    }
}
