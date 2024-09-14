using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Building.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQuery : IRequest<Result>
    {
        [JsonIgnore]
        public required int CommunityId { get; set; }
    }
    
}
