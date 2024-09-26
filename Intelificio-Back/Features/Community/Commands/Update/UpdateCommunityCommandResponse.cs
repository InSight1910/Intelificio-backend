namespace Backend.Features.Community.Commands.Update
{
    public class UpdateCommunityCommandResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int MunicipalityId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string RUT { get; set; }

    }
}
