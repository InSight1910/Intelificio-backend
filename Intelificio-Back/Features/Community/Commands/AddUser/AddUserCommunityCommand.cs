using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Community.Commands.Assign
{
    public class AddUserCommunityCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}
