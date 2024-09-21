using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Commands.Delete
{
    public class DeleteUnitCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}
