using Backend.Models.Enums;

namespace Backend.Features.Fine.Queries.GetAllByCommunity
{
    public class GetAllFinesByCommunityQueryResponse
    {
        public required int FineId { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }
        public int CommunityId { get; set; }
    }
}
