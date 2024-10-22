using Backend.Models.Enums;

namespace Backend.Features.Fine.Queries.GetFineById
{
    public class GetFineByIdQueryResponse
    {
        public required int FineId { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required FineDenomination Status { get; set; }
        public int CommunityId { get; set; }
    }
}
