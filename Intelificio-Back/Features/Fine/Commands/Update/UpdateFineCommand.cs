using Backend.Common.Response;
using Backend.Models.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Fine.Commands.Update
{
    public class UpdateFineCommand : IRequest<Result>
    {
        [JsonIgnore]
        public int FineId { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }
        public int CommunityId { get; set; }
    }
}
