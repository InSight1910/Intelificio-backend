using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.AssignedFines.Queries.GetAllAssignedFinesByCommunity
{
    public class GetAllAssignedFinesByCommunityIdQuery : IRequest<Result>
    {
        [JsonIgnore]
        public int CommunityId { get; set; }
    }
}

