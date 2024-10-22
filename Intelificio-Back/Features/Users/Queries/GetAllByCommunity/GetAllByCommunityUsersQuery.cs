using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Users.Queries.GetAllByCommunity
{
    public class GetAllByCommunityUsersQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int CommunityId { get; set; }
    }

}
