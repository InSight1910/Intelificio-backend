using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Community.Queries.GetUsersByCommunity
{
    public class GetUsersByCommunityQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}
