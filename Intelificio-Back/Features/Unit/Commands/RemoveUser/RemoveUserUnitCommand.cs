using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Unit.Commands.RemoveUser
{
    public class RemoveUserUnitCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int? UnitId { get; set; }
        public int UserId { get; set; }
    }
}