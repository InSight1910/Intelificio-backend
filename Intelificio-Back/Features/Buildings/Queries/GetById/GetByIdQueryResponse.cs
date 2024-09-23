namespace Backend.Features.Buildings.Queries.GetById
{
    public class GetByIDQueryResponse
    {
        public required string Name { get; set; }
        public required string CommunityName { get; set; }
        public required int Floors { get; set; }
    }
}
