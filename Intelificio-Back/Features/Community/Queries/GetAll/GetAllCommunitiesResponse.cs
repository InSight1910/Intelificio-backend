namespace Backend.Features.Community.Queries.GetAll
{
    public class GetAllCommunitiesResponse
    {   
        public required int Id { get; set; }
        public required string Name { get; set; }

        public required string Rut { get; set; }
        public required string Address { get; set; }

        public required string AdminName { get; set; }

        public required int AdminId { get; set; }

        public DateTime CreationDate { get; set; }
        public string Municipality { get; set; }

        public int MunicipalityId { get; set; }
        public string City { get; set; }

        public int CityId { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }


    }
}