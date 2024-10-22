using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Fine.Queries.GetAllByCommunity
{
    public class GetAllFinesByCommunityQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}
