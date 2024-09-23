namespace Backend.Features.Buildings.Queries.GetAllByCommunity
{
    public class GetAllByCommunityQueryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Floors { get; set; }
        public required int Id { get; set; }
        public required string CommunityName { get; set; }

        public required int CommunityId { get; set; }
        public required int Units { get; set; }

    }
}
