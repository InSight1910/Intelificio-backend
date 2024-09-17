namespace Backend.Features.Community.Queries.GetAll
{
    public class GetAllCommunitiesResponse
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public DateTime CreationDate { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
    }
}