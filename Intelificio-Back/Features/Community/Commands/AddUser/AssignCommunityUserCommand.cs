using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Community.Commands.Assign
{
    public class AssignCommunityUserCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}
