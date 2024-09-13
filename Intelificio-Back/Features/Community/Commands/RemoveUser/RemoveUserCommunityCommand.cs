using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Community.Commands.RemoveUser

{
    public class RemoveUserCommunityCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}
