using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Contact.Queries.GetallByCommunity
{
    public class GetAllContactsByCommunityQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int CommunityId { get; set; }
    }
}
