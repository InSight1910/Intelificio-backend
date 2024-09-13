using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string? Name { get; set; }
        public required string? Address { get; set; }
        public int? MunicipalityId { get; set; }
    }
}
